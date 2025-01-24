using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib.CoreService.Lib.Common;
using NaLib.CoreService.Lib.Data;
using NaLib.CoreService.Lib.Dto;

namespace NaLib.CoreService.API.Controllers
{

    [Route(ApiUrls.PermissionsAndRoles)]
    [ApiController]
    public class RolesAndPermissionsController : ControllerBase
    {

        private readonly NaLibCoreServiceDbContext _context;
        private readonly IConfiguration _configuration;

        public RolesAndPermissionsController(NaLibCoreServiceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        /// <summary>
        /// Creates a new role with role details.
        /// </summary>
        /// <param name="request">The request data to create a role.</param>
        /// <returns>A response indicating whether the role was successfully created.</returns>
        /// <response code="201">The role was created successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.CreateRole)]
        [ProducesResponseType(typeof(Response<RoleDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var role = new Role
                {
                    RoleName = request.RoleName,
                    Description = request.Description,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now)
                };

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var response = new RoleDto
                {

                    RoleName = role.RoleName,
                    Description = role.Description,
                    CreatedAt = role.CreatedAt
                };

                return this.SendApiResponse(response, "Role created successfully.");
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
        /// Retrieves a role by ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve.</param>
        /// <returns>A response containing the role.</returns>
        /// <response code="200">Role retrieved successfully.</response>
        /// <response code="404">Role not found.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpGet(ApiUrls.GetRole)]
        [ProducesResponseType(typeof(Response<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null)
                {
                    return this.SendApiError<object>(
                        null,
                        "RoleNotFound",
                        "The role was not found.",
                        StatusCodes.Status404NotFound);
                }

                var response = new RoleDto
                {

                    RoleName = role.RoleName,
                    Description = role.Description,
                    CreatedAt = role.CreatedAt
                };

                return this.SendApiResponse(response, "Role retrieved successfully.");
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
        /// Creates a new permission with permission details.
        /// </summary>
        /// <param name="request">The request data to create a permission.</param>
        /// <returns>A response indicating whether the permission was successfully created.</returns>
        /// <response code="201">The permission was created successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.CreatePermission)]
        [ProducesResponseType(typeof(Response<PermissionDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var permission = new Permission
                {
                    PermissionsName = request.PermissionsName,
                    Description = request.Description,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now)
                };

                await _context.Permissions.AddAsync(permission);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var response = new PermissionDto
                {

                    PermissionsName = permission.PermissionsName,
                    Description = permission.Description,
                    CreatedAt = permission.CreatedAt
                };

                return this.SendApiResponse(response, "Permission created successfully.");
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
        /// Retrieves a permission by ID.
        /// </summary>
        /// <param name="permissionId">The ID of the permission to retrieve.</param>
        /// <returns>A response containing the permission.</returns>
        /// <response code="200">Permission retrieved successfully.</response>
        /// <response code="404">Permission not found.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpGet(ApiUrls.GetPermission)]
        [ProducesResponseType(typeof(Response<PermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPermissionById(int permissionId)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(permissionId);
                if (permission == null)
                {
                    return this.SendApiError<object>(
                        null,
                        "PermissionNotFound",
                        "The permission was not found.",
                        StatusCodes.Status404NotFound);
                }

                var response = new PermissionDto
                {

                    PermissionsName = permission.PermissionsName,
                    Description = permission.Description,
                    CreatedAt = permission.CreatedAt
                };

                return this.SendApiResponse(response, "Permission retrieved successfully.");
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
        /// Assigns a role to permissions.
        /// </summary>
        /// <param name="request">The request data containing the role name and permission names.</param>
        /// <returns>A response indicating whether the role was successfully assigned to the permissions.</returns>
        /// <response code="201">The role was assigned to the permissions successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="404">Role or permission not found.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.AssignRoleToPermissions)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignRoleToPermissions([FromBody] AssignRoleToPermissionsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.RoleName);
            if (role == null)
            {
                return this.SendApiError<object>(
                    null,
                    "RoleNotFound",
                    "The role was not found.",
                    StatusCodes.Status404NotFound);
            }

            var permissions = await _context.Permissions.Where(p => request.PermissionNames.Contains(p.PermissionsName)).ToListAsync();
            if (permissions.Count != request.PermissionNames.Count)
            {
                return this.SendApiError<object>(
                    null,
                    "PermissionNotFound",
                    "One or more permissions were not found.",
                    StatusCodes.Status404NotFound);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var permission in permissions)
                {
                    var rolePermission = await _context.RolePermissions.FirstOrDefaultAsync(rp => rp.RoleId == role.Id && rp.PermissionsId == permission.Id);
                    if (rolePermission == null)
                    {
                        _context.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionsId = permission.Id });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return this.SendApiResponse("Role assigned to permissions successfully.", null);
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
    }
    }
