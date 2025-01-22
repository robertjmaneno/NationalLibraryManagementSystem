using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib.CoreService.Lib.Common;
using NaLib.CoreService.Lib.Data;
using NaLib.CoreService.Lib.Dto;
using NaLib.CoreService.Lib.Utils;
using static NaLib.CoreService.Lib.Utils.StaffHelpers;

namespace NaLib.CoreService.API.Controllers
{
    [Route(ApiUrls.login)]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly NaLibCoreServiceDbContext _context;

        public AuthentificationController(NaLibCoreServiceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        [HttpPost(ApiUrls.UserLogin)]
        [ProducesResponseType(typeof(Response<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Username == loginRequest.Username);

                if (user == null || !StaffHelpers.VerifyPassword(loginRequest.Password, user.PasswordHash))
                {
                    return this.SendApiError("Unauthorised", "An authorised access.", StatusCodes.Status401Unauthorized);
                }

             
                if (!user.IsActive)
                {
                    return this.SendApiError("Unauthorised", "An authorised access.", StatusCodes.Status401Unauthorized);
                }

               
                var token = JwtTokenGenerator.GenerateToken(user);

                var response = new LoginResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleId = user.RoleId,
                };

                return this.SendApiResponse(response, "Login successful.");
            }
            catch (Exception)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

    }
}
