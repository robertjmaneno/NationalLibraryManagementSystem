using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib.CoreService.Lib.Common;
using NaLib.CoreService.Lib.Data;
using NaLib.CoreService.Lib.Dto;
using NaLib.CoreService.Lib.Utils;

namespace NaLib.CoreService.API.Controllers
{
    /// <summary>
    /// Controller for managing members and their memberships.
    /// Provides endpoints for creating and managing member information and membership details.
    /// </summary>
    [Route(ApiUrls.Member)]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly NaLibCoreServiceDbContext _context;
        private readonly IConfiguration _configuration;

        public MembersController(NaLibCoreServiceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a new member with membership details.
        /// </summary>
        /// <param name="request">The request data to create a member and membership.</param>
        /// <returns>A response indicating whether the member and membership were successfully created.</returns>
        /// <response code="201">The member and membership were created successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.CreateMember)]
        [ProducesResponseType(typeof(Response<MemberWithMembershipResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMemberWithMembership([FromBody] CreateMemberWithMembershipRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var member = new Member
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    PostalAddress = request.PostalAddress,
                    PhysicalAddress = request.PhysicalAddress,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now)
                };

                await _context.Members.AddAsync(member);
                await _context.SaveChangesAsync();

                var membership = new Membership
                {
                    MembershipId = GenerateMembershipIdHelper.GenerateMembershipId(),
                    MemberId = member.MemberId,
                    EnrollmentDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = request.Status,
                    OverDueCount = 0,
                    PreferredGenres = request.PreferredGenres,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now)
                };

                await _context.Memberships.AddAsync(membership);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var response = new MemberWithMembershipResponseDto
                {
                    MemberId = member.MemberId,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    PhoneNumber = member.PhoneNumber,
                    PostalAddress = member.PostalAddress,
                    PhysicalAddress = member.PhysicalAddress,
                    CreatedAt = member.CreatedAt,
                    Membership = new MembershipResponseDto
                    {
                        MembershipId = membership.MembershipId,
                        EnrollmentDate = membership.EnrollmentDate,
                        Status = membership.Status,
                        OverDueCount = membership.OverDueCount,
                        PreferredGenres = membership.PreferredGenres
                    }
                };

                return this.SendApiResponse(response, "Member with membership created successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a list of all members with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination (default is 1).</param>
        /// <param name="pageSize">The number of members per page (default is 10).</param>
        /// <returns>A list of members with pagination, including membership details.</returns>
        /// <response code="200">A list of members with membership details successfully returned.</response>
        [HttpGet(ApiUrls.GetAllMembers)]
        public async Task<IActionResult> GetMembers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var totalMembers = await _context.Members.CountAsync();

            var members = await _context.Members
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MemberWithMembershipResponseDto
                {
                    MemberId = m.MemberId,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber,
                    PostalAddress = m.PostalAddress,
                    PhysicalAddress = m.PhysicalAddress,
                    CreatedAt = m.CreatedAt,
                    Membership = new MembershipResponseDto
                    {
                        MembershipId = m.Membership.MembershipId,
                        EnrollmentDate = m.Membership.EnrollmentDate,
                        Status = m.Membership.Status,
                        OverDueCount = m.Membership.OverDueCount,
                        PreferredGenres = m.Membership.PreferredGenres
                    }
                })
                .ToListAsync();

            var pagedResponse = new PagedResponse<MemberWithMembershipResponseDto>
            {
                TotalCount = totalMembers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = members
            };

            return this.SendApiResponse(pagedResponse, "Members with memberships retrieved successfully.");
        }





        /// <summary>
        /// Updates an existing member's details.
        /// </summary>
        /// <param name="id">The ID of the member to update.</param>
        /// <param name="request">The request data containing the updated member details.</param>
        /// <returns>A response indicating whether the member was successfully updated.</returns>
        /// <response code="200">The member was updated successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="404">Member not found.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPut(ApiUrls.UpdateMember)]
        [ProducesResponseType(typeof(Response<MemberWithMembershipResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] UpdateMemberRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return this.SendApiError<object>(null, "Member not found", "The member with the specified ID was not found.", StatusCodes.Status404NotFound);
            }

            try
            {             
                member.FirstName = request.FirstName;
                member.LastName = request.LastName;
                member.Email = request.Email;
                member.PhoneNumber = request.PhoneNumber;
                member.PostalAddress = request.PostalAddress;
                member.PhysicalAddress = request.PhysicalAddress;

                _context.Members.Update(member);
                await _context.SaveChangesAsync();

                var response = new MemberWithMembershipResponseDto
                {
                    MemberId = member.MemberId,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    PhoneNumber = member.PhoneNumber,
                    PostalAddress = member.PostalAddress,
                    PhysicalAddress = member.PhysicalAddress,
                    CreatedAt = member.CreatedAt,
                    Membership = new MembershipResponseDto
                    {

                        Status = member.Membership.Status,
                        OverDueCount = member.Membership.OverDueCount,
                        PreferredGenres = member.Membership.PreferredGenres
                    }
                };

                return this.SendApiResponse(response, "Member updated successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Searches for members by first name, last name, or membership ID.
        /// At least one of the parameters (first name, last name, or membership ID) must be provided for the search.
        /// </summary>
        /// <param name="firstName">The first name of the member to search for (optional).</param>
        /// <param name="lastName">The last name of the member to search for (optional).</param>
        /// <param name="membershipId">The membership ID of the member to search for (optional).</param>
        /// <returns>A list of members matching the search criteria, including membership details.</returns>
        /// <response code="200">Search results successfully returned.</response>
        /// <response code="400">At least one search parameter is required.</response>
        /// <response code="404">No members found matching the criteria.</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(Response<List<MemberWithMembershipResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchMembers(
            [FromQuery] string? firstName,
            [FromQuery] string? lastName,
            [FromQuery] string? membershipId)
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(membershipId))
            {
                return this.SendApiError<object>(null, "Bad Request", "At least one search parameter (first name, last name, or membership ID) is required.", StatusCodes.Status400BadRequest);
            }

            var query = _context.Members.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(m => m.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(m => m.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(membershipId))
            {
                query = query.Where(m => m.Membership.MembershipId.Contains(membershipId));
            }

            var members = await query
                .Include(m => m.Membership) 
                .Select(m => new MemberWithMembershipResponseDto
                {
                    MemberId = m.MemberId,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber,
                    PostalAddress = m.PostalAddress,
                    PhysicalAddress = m.PhysicalAddress,
                    CreatedAt = m.CreatedAt,
                    Membership = new MembershipResponseDto
                    {
                        MembershipId = m.Membership.MembershipId,
                        EnrollmentDate = m.Membership.EnrollmentDate,
                        Status = m.Membership.Status,
                        OverDueCount = m.Membership.OverDueCount,
                        PreferredGenres = m.Membership.PreferredGenres
                    }
                })
                .ToListAsync();

            if (!members.Any())
            {
                return this.SendApiError<object>(null, "Not Found", "No members match the search criteria.", StatusCodes.Status404NotFound);
            }

            return this.SendApiResponse(members, "Search results returned successfully.");
        }




        /// <summary>
        /// Deletes a member and their associated membership.
        /// </summary>
        /// <param name="id">The ID of the member to delete.</param>
        /// <returns>A response indicating whether the member and membership were successfully deleted.</returns>
        /// <response code="200">The member and membership were deleted successfully.</response>
        /// <response code="404">Member not found.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpDelete(ApiUrls.DeleteMember)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members
                .Include(m => m.Membership) 
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return this.SendApiError<object>(null, "Member not found", "The member with the specified ID was not found.", StatusCodes.Status404NotFound);
            }

            try
            {
                if (member.Membership != null)
                {
                    _context.Memberships.Remove(member.Membership);
                }

                _context.Members.Remove(member);
                await _context.SaveChangesAsync();

                return this.SendApiResponse<object>(null, "Member and membership deleted successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Gets a member by ID with their associated membership details.
        /// </summary>
        /// <param name="id">The ID of the member to retrieve.</param>
        /// <returns>The member with their membership details.</returns>
        /// <response code="200">The member and their membership details retrieved successfully.</response>
        /// <response code="404">Member not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<MemberWithMembershipResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member = await _context.Members
                .Include(m => m.Membership) 
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return this.SendApiError<object>(null, "Member not found", "The member with the specified ID was not found.", StatusCodes.Status404NotFound);
            }

            var response = new MemberWithMembershipResponseDto
            {
                MemberId = member.MemberId,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                PostalAddress = member.PostalAddress,
                PhysicalAddress = member.PhysicalAddress,
                CreatedAt = member.CreatedAt,
                Membership = new MembershipResponseDto
                {
                    MembershipId = member.Membership.MembershipId,
                    EnrollmentDate = member.Membership.EnrollmentDate,
                    Status = member.Membership.Status,
                    OverDueCount = member.Membership.OverDueCount,
                    PreferredGenres = member.Membership.PreferredGenres
                }
            };

            return this.SendApiResponse(response, "Member and membership details retrieved successfully.");
        }

    }
}
