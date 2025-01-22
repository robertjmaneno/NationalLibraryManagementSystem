using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaLib.CoreService.Lib.Data;
using NaLib.CoreService.Lib.Common;
using System;
using System.Threading.Tasks;
using NaLib.CoreService.Lib.Dto;
using NaLib.CoreService.Lib.Utils;

namespace NaLib.CoreService.API.Controllers
{
    [Route(ApiUrls.Staff)]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly NaLibCoreServiceDbContext _context;

        public StaffController(NaLibCoreServiceDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        [HttpPost(ApiUrls.CreateStaff)]
        [ProducesResponseType(typeof(Response<UserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hashedPassword = StaffHelpers.HashPassword(userRequest.Password);
                var newUser = new User
                {
                    Username = userRequest.Username,
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    PasswordHash = hashedPassword,
                    Email = userRequest.Email,
                    PhoneNumber = userRequest.PhoneNumber,
                    DateOfBirth = userRequest.DateOfBirth,
                    IsPasswordExpired = false,
                    LastPasswordChangeDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = null,
                    LibraryId = userRequest.LibraryId,
                    RoleId = userRequest.RoleId,
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var userResponse = new UserDto
                {
                    Id = newUser.Id,
                    Username = newUser.Username,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber,
                    DateOfBirth = newUser.DateOfBirth,
                    LibraryId = newUser.LibraryId,
                    RoleId = newUser.RoleId,
                };

                return this.SendApiResponse(userResponse, "User created successfully.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }






        /// <summary>
        /// Fetches user details by ID.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns>User details.</returns>
        /// <response code="200">User details fetched successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpGet(ApiUrls.GetStaff)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }
    }
}
