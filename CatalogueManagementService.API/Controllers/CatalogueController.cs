using CatalogueManagementService.Lib.Data;
using CatalogueManagementService.LIb.Common;
using CatalogueManagementService.LIb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogueManagementService.API.Controllers
{
    [Route(KnowbUrls.Catalogue)]
    [ApiController]
    public class CatalogueController : ControllerBase
    {
        private readonly IMongoCollection<ResourceCollection> _resource;

        public CatalogueController(CatalogueContext context)
        {
            _resource = context.GetCollection<ResourceCollection>("ResourceCollections");
        }

        /// <summary>
        /// Creates a new library resource.
        /// </summary>
        /// <param name="resource">Details of the new library resource.</param>
        /// <returns>
        /// The created library resource details.
        /// </returns>
        /// <response code="201">Library resource created successfully.</response>
        /// <response code="400">Validation error or bad request.</response>
        /// <response code="500">An error occurred while creating the library resource.</response>
        [HttpPost(KnowbUrls.CreateResource)]
        [ProducesResponseType(typeof(ResourceCollection), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateResource([FromBody] ResourceCollection resource)
        {
            if (resource == null)
            {
                return BadRequest("Resource is null.");
            }

            try
            {
                await _resource.InsertOneAsync(resource);
                return CreatedAtAction(nameof(GetResource), new { id = resource.Id.ToString() }, resource);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the library resource.");
            }
        }

        /// <summary>
        /// Gets a specific library resource by ID.
        /// </summary>
        /// <param name="id">ID of the resource to fetch.</param>
        /// <returns>Library resource with the specified ID.</returns>
        /// <response code="200">Returns the requested resource.</response>
        /// <response code="400">Invalid ID format.</response>
        /// <response code="404">Resource not found.</response>
        /// <response code="500">An error occurred while fetching the resource.</response>
        [HttpGet(KnowbUrls.GetResource)]
        [ProducesResponseType(typeof(ResourceCollection), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResourceCollection>> GetResource(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid ID format.");

            try
            {
                var resource = await _resource.Find(r => r.Id == objectId).FirstOrDefaultAsync();

                if (resource == null)
                    return NotFound("Resource not found.");

                return Ok(resource);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the library resource.");
            }
        }

        /// <summary>
        /// Updates an existing library resource by ID.
        /// </summary>
        /// <param name="id">ID of the resource to update.</param>
        /// <param name="updatedResource">Updated details of the library resource.</param>
        /// <returns>Updated library resource details.</returns>
        /// <response code="204">Library resource updated successfully.</response>
        /// <response code="400">Invalid ID format or bad request.</response>
        /// <response code="404">Resource not found.</response>
        /// <response code="500">An error occurred while updating the resource.</response>
        [HttpPut(KnowbUrls.UpdateResource)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateResource(string id, ResourceCollection updatedResource)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid ID format.");

            updatedResource.Id = objectId;

            try
            {
                var result = await _resource.ReplaceOneAsync(r => r.Id == objectId, updatedResource);

                if (result.MatchedCount == 0)
                    return NotFound("Resource not found.");

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the library resource.");
            }
        }

        /// <summary>
        /// Deletes a library resource by ID.
        /// </summary>
        /// <param name="id">ID of the resource to delete.</param>
        /// <returns>Confirmation of deletion.</returns>
        /// <response code="204">Library resource deleted successfully.</response>
        /// <response code="400">Invalid ID format.</response>
        /// <response code="404">Resource not found.</response>
        /// <response code="500">An error occurred while deleting the resource.</response>
        [HttpDelete(KnowbUrls.DeleteResource)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteResource(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid ID format.");

            try
            {
                var result = await _resource.DeleteOneAsync(r => r.Id == objectId);

                if (result.DeletedCount == 0)
                    return NotFound("Resource not found.");

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the library resource.");
            }
        }


        /// <summary>
        /// Searches for library resources based on filters.
        /// </summary>
        /// <param name="title">Optional filter: Title of the resource.</param>
        /// <param name="type">Optional filter: Type of the resource.</param>
        /// <param name="genres">Optional filter: Genres of the resource.</param>
        /// <returns>List of library resources matching the filters.</returns>
        /// <response code="200">Returns the matching resources.</response>
        /// <response code="400">Invalid query parameters.</response>
        /// <response code="500">An error occurred while searching for resources.</response>
        [HttpGet(KnowbUrls.SearchResources)]
        [ProducesResponseType(typeof(IEnumerable<ResourceCollection>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchResources(
            [FromQuery] string? title = null,
            [FromQuery] string? type = null,
            [FromQuery] List<string>? genres = null)
        {
            try
            {
                var filterBuilder = Builders<ResourceCollection>.Filter;
                var filters = new List<FilterDefinition<ResourceCollection>>();

                if (!string.IsNullOrEmpty(title))
                {
                    filters.Add(filterBuilder.Regex("title", new BsonRegularExpression(title, "i")));
                }

                if (!string.IsNullOrEmpty(type))
                {
                    filters.Add(filterBuilder.Eq("type", type));
                }

                if (genres != null && genres.Count > 0)
                {
                    filters.Add(filterBuilder.AnyIn("genres", genres));
                }

                var filter = filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<ResourceCollection>.Empty;

                var resources = await _resource.Find(filter).ToListAsync();
                return Ok(resources);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching for library resources.");
            }
        }


        /// <summary>
        /// Checks the availability and in-library-only status of a resource.
        /// </summary>
        /// <param name="id">ID of the resource to check.</param>
        /// <returns>Resource availability details.</returns>
        /// <response code="200">Returns availability details of the resource.</response>
        /// <response code="400">Invalid ID format.</response>
        /// <response code="404">Resource not found.</response>
        /// <response code="500">An error occurred while checking the resource availability.</response>
        [HttpGet(KnowbUrls.CheckResourceAvailability)]
        [ProducesResponseType(typeof(AvailabilityInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckResourceAvailability(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid ID format.");

            try
            {
                var resource = await _resource.Find(r => r.Id == objectId)
                                              .Project(r => r.Availability)
                                              .FirstOrDefaultAsync();

                if (resource == null)
                    return NotFound("Resource not found.");

                return Ok(resource);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while checking the resource availability.");
            }
        }


    }
}
