using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib.CoreService.Lib.Common;
using NaLib.CoreService.Lib.Data;
using NaLib.CoreService.Lib.Dto;
using NaLib.CoreService.Lib.Interfaces;
using NaLib.CoreService.Lib.Utils;

namespace NaLib.CoreService.API.Controllers
{
    [Route(ApiUrls.LendingTransaction)]
    [ApiController]
    public class LendingTransactionController : ControllerBase
    {
        private readonly NaLibCoreServiceDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICatalogService _checkCatalogueService;

        public LendingTransactionController(NaLibCoreServiceDbContext context, IConfiguration configuration, ICatalogService checkCatalogueService)
        {
            _context = context;
            _configuration = configuration;
            _checkCatalogueService = checkCatalogueService;
        }

        /// <summary>
        /// Handles the checkout of a single resource for a member.
        /// </summary>
        /// <param name="request">The request data for checking out a resource.</param>
        /// <returns>A response indicating whether the resource was successfully checked out.</returns>
        /// <response code="200">Resource checked out successfully.</response>
        /// <response code="400">The resource is not available for lending.</response>
        /// <response code="404">The member with the provided membership ID or the resource was not found.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.CheckoutResources)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckoutResource([FromBody] LendingCheckoutRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var member = await _context.Memberships.FirstOrDefaultAsync(m => m.MembershipId == request.MembershipId);
                if (member == null)
                {
                    return this.SendApiError<object>(
                        "Membership not found",
                        "MembershipId",
                        "The provided MembershipId does not exist.",
                        StatusCodes.Status404NotFound
                    );
                }

                var catalogResponse = await _checkCatalogueService.GetResourceDetailsAsync(request.LibraryResourceId);
                if (catalogResponse == null)
                {
                    return this.SendApiError<object>(
                        "Resource not found in catalog.",
                        "LibraryResourceId",
                        "The provided LibraryResourceId does not exist in the catalog.",
                        StatusCodes.Status404NotFound
                    );
                }

                if (!catalogResponse.IsBorrowable)
                {
                    return this.SendApiResponse(
                        "Resource is not borrowable",
                        null
                    );
                }
                else if (catalogResponse.BorrowStatus != "Available")
                {
                    return this.SendApiResponse(
                        "Resource is already borrowed",
                        null
                    );
                }

                var status = "Borrowed";
                var updateResponse = await _checkCatalogueService.UpdateResourceStatusAsync(request.LibraryResourceId, status);
                if (!updateResponse)
                {
                    await transaction.RollbackAsync();
                    return this.SendApiError<object>(
                        null,
                        "UpdateError",
                        "Failed to update the resource status to 'Borrowed'.",
                        StatusCodes.Status500InternalServerError
                    );
                }

                var lendingTransaction = new LendingTransaction
                {
                    MemberId = member.MemberId,
                    ResourceId = request.LibraryResourceId,
                    LendingDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(catalogResponse.BorrowLimitInDays)),
                    LendingStatus = "Active",
                    IsAlertIssued = false,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                };

                _context.LendingTransactions.Add(lendingTransaction);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return this.SendApiResponse("Resource checked out successfully.", null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    


/// <summary>
/// Handles the return of resources by a member.
/// </summary>
/// <param name="request">The request body containing the resources to be returned, including their condition.</param>
/// <returns>
/// Returns a status indicating whether the resources were successfully returned or if there were errors. 
/// If the membership or any resource is invalid, appropriate error messages are returned.
/// </returns>
/// <response code="200">Returns a success message when resources are successfully returned.</response>
/// <response code="400">Returns a bad request message when some resources could not be processed.</response>
/// <response code="404">Returns a not found message if the membership is not found.</response>
/// <response code="500">Returns an internal server error message for any unexpected issues.</response>
[HttpPost(ApiUrls.ReturnResources)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReturnResources([FromBody] ReturnResourceRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(
                    null,
                    "Validation error",
                    ModelState
                );
            }

            await using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var member = await _context.Memberships
                    .FirstOrDefaultAsync(m => m.MembershipId == request.MembershipId);
                if (member == null)
                {
                    return this.SendApiError<object>(
                        "Membership not found",
                        "MembershipId",
                        "The provided MembershipId does not exist.",
                        StatusCodes.Status404NotFound
                    );
                }

                var invalidResources = new List<string>();
                var transactionsToUpdate = new List<LendingTransaction>();
                int newOverdueCount = 0;

                foreach (var resourceReturnInfo in request.ResourceReturnInfo)
                {
                    var resourceId = resourceReturnInfo.ResourceId;
                    var condition = resourceReturnInfo.Condition;

                    var lendingTransaction = await _context.LendingTransactions
                        .FirstOrDefaultAsync(t => t.ResourceId == resourceId
                                                  && t.MemberId == member.MemberId
                                                  && t.LendingStatus == "Active");

                    if (lendingTransaction == null)
                    {
                        invalidResources.Add($"Resource ID {resourceId}: No active transaction found.");
                        continue;
                    }

                    if (DateOnly.FromDateTime(DateTime.UtcNow) > lendingTransaction.DueDate)
                    {
                        newOverdueCount++;
                    }

                    
                    lendingTransaction.LendingStatus = "Returned";
                    lendingTransaction.ReturnDate = DateOnly.FromDateTime(DateTime.UtcNow);
                    lendingTransaction.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                    lendingTransaction.ResourceCondition = condition;
                    transactionsToUpdate.Add(lendingTransaction);

                    
                    var updateResponse = await _checkCatalogueService.UpdateResourceStatusAsync(resourceId, "Available");
                    if (!updateResponse)
                    {
                        await dbTransaction.RollbackAsync();
                        return this.SendApiError<object>(
                            null,
                            "UpdateError",
                            "Failed to update the resource status to 'Available'.",
                            StatusCodes.Status500InternalServerError
                        );
                    }
                }

                if (invalidResources.Any())
                {
                    return this.SendApiError<object>(
                        "Some resources could not be processed.",
                        "ResourceId",
                        string.Join(", ", invalidResources),
                        StatusCodes.Status400BadRequest
                    );
                }

                if (newOverdueCount > 0)
                {
                    var currentOverdueCount = member.OverDueCount;
                    member.OverDueCount = currentOverdueCount + newOverdueCount;
                    member.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                    _context.Memberships.Update(member);
                }

                _context.LendingTransactions.UpdateRange(transactionsToUpdate);
                await _context.SaveChangesAsync();

                await dbTransaction.CommitAsync();

                return this.SendApiResponse("Resources returned successfully.", null);
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}