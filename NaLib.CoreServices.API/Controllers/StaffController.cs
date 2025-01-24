using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaLib.CoreService.Lib.Data;
using NaLib.CoreService.Lib.Common;
using System;
using System.Threading.Tasks;
using NaLib.CoreService.Lib.Dto;
using NaLib.CoreService.Lib.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace NaLib.CoreService.API.Controllers
{
    [Route(ApiUrls.Staff)]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly NaLibCoreServiceDbContext _context;
        private readonly ILogger<StaffController> _logger;

        public StaffController(NaLibCoreServiceDbContext context, ILogger<StaffController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="userRequest">The request payload containing user details.</param>
        /// <returns>
        /// An IActionResult indicating the result of the operation:
        /// - 201 Created if the user is successfully created.
        /// - 400 Bad Request if the request validation fails or required data is missing.
        /// - 500 Internal Server Error if an unexpected error occurs.
        /// </returns>
        /// <remarks>
        /// A database transaction is used to ensure atomicity of the operation.
        /// </remarks>
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

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == userRequest.RoleName);
                if (role == null)
                {
                    return this.SendApiError("RoleNotFound", $"Role '{userRequest.RoleName}' does not exist.", StatusCodes.Status400BadRequest);
                }


                var library = await _context.Libraries.FirstOrDefaultAsync(l => l.LibraryName == userRequest.LibraryName);
                if (library == null)
                {
                    return this.SendApiError("LibraryNotFound", $"Library '{userRequest.LibraryName}' does not exist.", StatusCodes.Status400BadRequest);
                }

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
                    LibraryId = library.Id,
                    RoleId = role.Id,
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
        /// Retrieves a list of all users in the system.
        /// </summary>
        /// <remarks>
        /// This method fetches all user details from the database and returns them in a structured format. 
        /// If no users are found, a `404 Not Found` response is returned. In case of an error, a `500 Internal Server Error` is returned.
        /// </remarks>
        /// <returns>
        /// An `IActionResult` containing a response with the list of all users. Returns `200 OK` with the list if successful, or `404 Not Found` if no users are found.
        /// </returns>
        [HttpGet(ApiUrls.GetAllStaffs)]
        [ProducesResponseType(typeof(Response<List<UserDetailDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {

                var users = await _context.VwUserDetails.ToListAsync();


                if (users == null || users.Count == 0)
                {
                    return this.SendApiError("NoUsersFound", "No users found in the system.", StatusCodes.Status404NotFound);
                }


                var userDetailDtos = users.Select(u => new UserDetailDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    DateOfBirth = u.DateOfBirth,
                    IsActive = u.IsActive,
                    UserCreatedAt = u.UserCreatedAt,
                    LibraryName = u.LibraryName,
                    RoleName = u.RoleName,
                    YearsOfExperience = u.YearsOfExperience,
                    GradeName = u.GradeName,
                    QualificationName = u.QualificationName,
                    SkillName = u.SkillName
                }).ToList();


                return this.SendApiResponse(userDetailDtos, "Users retrieved successfully.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while retrieving the users.");


                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }




        /// <summary>
        /// Fetches user details by ID from the User view.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns>User details.</returns>
        /// <response code="200">User details fetched successfully.</response>
        /// <response code="404">User not found.</response>
        [HttpGet(ApiUrls.GetStaff)]
        [ProducesResponseType(typeof(Response<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]

        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {

                var userDetails = await _context.Set<VwUserDetail>()
                                                .Where(u => u.UserId == id)
                                                .FirstOrDefaultAsync();

                if (userDetails == null)
                {
                    return this.SendApiError<object>(null, "UserNotFound", "User not found.", StatusCodes.Status404NotFound);
                }


                var userResponse = new
                {
                    UserId = userDetails.UserId,
                    Username = userDetails.Username,
                    FirstName = userDetails.FirstName,
                    LastName = userDetails.LastName,
                    Email = userDetails.Email,
                    PhoneNumber = userDetails.PhoneNumber,
                    DateOfBirth = userDetails.DateOfBirth,
                    UserCreatedAt = userDetails.UserCreatedAt,
                    LibraryName = userDetails.LibraryName,
                    RoleName = userDetails.RoleName,
                    YearsOfExperience = userDetails.YearsOfExperience,
                    GradeName = userDetails.GradeName,
                    QualificationName = userDetails.QualificationName,
                    SkillName = userDetails.SkillName
                };

                return this.SendApiResponse(userResponse, "User details fetched successfully.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while fetching user details by ID.");

                return this.SendApiError<object>(null, "DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }
    }
}