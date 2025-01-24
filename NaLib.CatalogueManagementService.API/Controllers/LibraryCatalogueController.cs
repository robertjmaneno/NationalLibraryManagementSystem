using AutoMapper;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using NaLib.CatalogueManagementService.API.Services;
using NaLib.CatalogueManagementService.Lib.Dto;
using NaLib.CatalogueManagementService.API.Extensions;
using NaLib.CatalogueManagementService.Lib.Common;
using Microsoft.AspNetCore.Authorization;

namespace NaLib.CatalogueManagementService.API.Controllers
{
    [Route(ApiUrls.LibraryResource)]
    [ApiController]
    public class LibraryCatalogueController : ControllerBase
    {
        private readonly LibraryResourceService _service;
        private readonly IMapper _mapper;

        public LibraryCatalogueController(LibraryResourceService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all resources.
        /// </summary>
        /// <returns>A list of all resources.</returns>
        /// <response code="200">The resources were retrieved successfully.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [HttpGet(ApiUrls.getAllLibraryResources)]
        [ProducesResponseType(typeof(Response<List<LibraryResourceDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]

        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var resources = await _service.GetAllAsync();
                var resourceDtos = _mapper.Map<List<LibraryResourceDto>>(resources);
                return this.SendApiResponse(resourceDtos, "Resources retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError<object>(
                    null,
                    "ServerError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a new library resource.
        /// </summary>
        /// <param name="resourceDto">The data for creating a new library resource.</param>
        /// <returns>The details of the created library resource.</returns>
        /// <response code="201">The library resource was created successfully.</response>
        /// <response code="400">Validation error in the provided data.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [HttpPost(ApiUrls.createLibraryResource)]
        [ProducesResponseType(typeof(CreateLibraryResourceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]

        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateLibraryResourceDto resourceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { statusCode = 400, message = "Validation error", errors = ModelState });
            }

            try
            {

                var resource = _mapper.Map<LibraryResource>(resourceDto);
                await _service.CreateAsync(resource);
                var createdResourceDto = _mapper.Map<CreateLibraryResourceDto>(resource);
                return CreatedAtAction(nameof(GetById), new { id = resource.Id }, new
                {
                    statusCode = 201,
                    message = "Library resource created successfully.",
                    data = createdResourceDto
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    statusCode = 500,
                    message = "An error occurred while processing your request."
                });
            }
        }

        /// <summary>
        /// Retrieves a specific library resource by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the library resource.</param>
        /// <returns>The library resource with the specified ID.</returns>
        /// <response code="200">The resource was retrieved successfully.</response>
        /// <response code="404">The resource was not found.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [HttpGet(ApiUrls.getLibraryResource)]
        [ProducesResponseType(typeof(Response<LibraryResourceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]

        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                
                var objectId = ObjectId.Parse(id);
                var resource = await _service.GetByIdAsync(objectId);

                if (resource == null)
                {
                    return this.SendApiError<object>(
                        null,
                        "NotFound",
                        "The requested resource could not be found.",
                        StatusCodes.Status404NotFound);
                }

                var resourceDto = _mapper.Map<LibraryResourceDto>(resource);
                return this.SendApiResponse(resourceDto, "Resource retrieved successfully.");
            }
            catch (FormatException)
            {
                return this.SendApiError<object>(
                    null,
                    "InvalidId",
                    "The provided ID is not in a valid format.",
                    StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return this.SendApiError<object>(
                    null,
                    "ServerError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Updates an existing resource by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to update.</param>
        /// <param name="resourceDto">The data to update the resource with.</param>
        /// <returns>The updated resource details if successful.</returns>
        /// <response code="200">The resource was updated successfully.</response>
        /// <response code="400">Invalid ID format provided.</response>
        /// <response code="404">The resource with the specified ID was not found.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [HttpPut(ApiUrls.updateLibraryResource)]
        [ProducesResponseType(typeof(Response<LibraryResourceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateLibraryResourceDto resourceDto)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return this.SendApiError<object>(null, "Invalid ID format", null, StatusCodes.Status400BadRequest);
            }

            try
            {
                var existingResource = await _service.GetByIdAsync(objectId);
                if (existingResource == null)
                {
                    return this.SendApiError<object>(null, "Resource not found", null, StatusCodes.Status404NotFound);
                }

                _mapper.Map(resourceDto, existingResource);
                await _service.UpdateAsync(objectId, existingResource);

                var updatedResourceDto = _mapper.Map<LibraryResourceDto>(existingResource);
                return this.SendApiResponse(updatedResourceDto, "Resource updated successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError<object>(
                    null,
                    "ServerError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Deletes an existing resource by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the resource to delete.</param>
        /// <returns>A response indicating the outcome of the deletion.</returns>
        /// <response code="204">The resource was deleted successfully.</response>
        /// <response code="400">Invalid ID format provided.</response>
        /// <response code="404">The resource with the specified ID was not found.</response>
        /// <response code="500">An error occurred while processing the request.</response>
        [HttpDelete(ApiUrls.deleteLibraryResource)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return this.SendApiError<object>(null, "Invalid ID format", null, StatusCodes.Status400BadRequest);
            }

            try
            {
                var existingResource = await _service.GetByIdAsync(objectId);
                if (existingResource == null)
                {
                    return this.SendApiError<object>(null, "Resource not found", null, StatusCodes.Status404NotFound);
                }

                await _service.DeleteAsync(objectId);
                return this.SendApiResponse("Resource deleted successfully.", null);
            }
            catch (Exception ex)
            {
                return this.SendApiError<object>(
                    null,
                    "ServerError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }

    }
}
