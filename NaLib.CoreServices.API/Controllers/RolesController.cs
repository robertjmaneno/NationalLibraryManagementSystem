using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaLib.CoreService.Lib.Common;
using NaLib.CoreService.Lib.Data;
using System;
using System.Threading.Tasks;

namespace NaLib.CoreService.API.Controllers
{
    [Route(ApiUrls.Roles)]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly NaLibCoreServiceDbContext _context;

        public RolesController(NaLibCoreServiceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="role">Details of the new role.</param>
        /// <returns>
        /// The created role details.
        /// </returns>
        /// <response code="201">Role created successfully.</response>
        /// <response code="400">Validation error or bad request.</response>
        /// <response code="500">An error occurred while creating the role.</response>
        [HttpPost(ApiUrls.CreateRole)]
        [ProducesResponseType(typeof(Role), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            if (role == null)
                return BadRequest("Role details cannot be null.");

            try
            {
                role.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the role.");
            }
        }

        /// <summary>
        /// Fetches role details by ID.
        /// </summary>
        /// <param name="id">ID of the role.</param>
        /// <returns>Role details.</returns>
        /// <response code="200">Role details fetched successfully.</response>
        /// <response code="404">Role not found.</response>
        [HttpGet(ApiUrls.GetRole)]
        [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound("Role not found.");

            return Ok(role);
        }



        /// <summary>
        /// Create a new permission.
        /// </summary>
        [HttpPost(ApiUrls.CreatePermission)]
        [ProducesResponseType(typeof(Permission), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePermission([FromBody] Permission permission)
        {
            if (permission == null)
                return BadRequest("Permission data is required.");

            try
            {
                permission.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPermissionById), new { id = permission.Id }, permission);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the permission.");
            }
        }

        /// <summary>
        /// Get all permissions.
        /// </summary>
        [HttpGet(ApiUrls.GetPermission)]
        [ProducesResponseType(typeof(IQueryable<Permission>), StatusCodes.Status200OK)]
        public IActionResult GetAllPermissions()
        {
            return Ok(_context.Permissions.ToList());
        }

        /// <summary>
        /// Get a specific permission by ID.
        /// </summary>
        [HttpGet(ApiUrls.GetPermissionID)]
        [ProducesResponseType(typeof(Permission), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);

            if (permission == null)
                return NotFound("Permission not found.");

            return Ok(permission);
        }

        /// <summary>
        /// Assign a permission to a role.
        /// </summary>
        [HttpPost(ApiUrls.CreateRolePermission)]
        [ProducesResponseType(typeof(RolePermission), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] RolePermission rolePermission)
        {
            if (rolePermission == null)
                return BadRequest("RolePermission data is required.");

            try
            {
                rolePermission.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _context.RolePermissions.Add(rolePermission);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRolePermissionById), new { id = rolePermission.Id }, rolePermission);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while assigning the permission to the role.");
            }
        }

        /// <summary>
        /// Get all role-permission assignments.
        /// </summary>
        [HttpGet(ApiUrls.GetRolePermission)]
        [ProducesResponseType(typeof(IQueryable<RolePermission>), StatusCodes.Status200OK)]
        public IActionResult GetAllRolePermissions()
        {
            return Ok(_context.RolePermissions
                .Select(rp => new
                {
                    rp.Id,
                    rp.RoleId,
                    rp.PermissionsId,
                    rp.CreatedAt,
                    rp.UpdatedAt
                }).ToList());
        }

        /// <summary>
        /// Get a specific role-permission assignment by ID.
        /// </summary>
        [HttpGet(ApiUrls.GetRolePermissionID)]
        [ProducesResponseType(typeof(RolePermission), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRolePermissionById(int id)
        {
            var rolePermission = await _context.RolePermissions.FindAsync(id);

            if (rolePermission == null)
                return NotFound("RolePermission not found.");

            return Ok(rolePermission);
        }
    }
}
