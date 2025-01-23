using MongoDB.Bson;
using MongoDB.Driver;
using NaLib.CatalogueManagementService.Lib.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaLib.CatalogueManagementService.API.Services
{
    public class LibraryResourceService
    {
        private readonly IMongoCollection<LibraryResource> _libraryResources;

        public LibraryResourceService(IMongoDatabase database)
        {
            _libraryResources = database.GetCollection<LibraryResource>("LibraryResources");
        }

        public async Task<LibraryResource> GetByIdAsync(ObjectId id) 
        {
            return await _libraryResources.Find(resource => resource.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(LibraryResource resource)
        {
            switch (resource.ResourceType.ToLower())
            {
                case "book":
                    resource.IsBorrowable = true;
                    resource.BorrowRules = new BorrowRule { BorrowLimitInDays = 10 };
                    break;

                case "newspaper":
                case "article":
                    resource.IsBorrowable = false;
                    resource.BorrowRules = new BorrowRule { BorrowLimitInDays = 1 };
                    break;

                default:
                    throw new ArgumentException($"Unsupported ResourceType: {resource.ResourceType}");
            }

            await _libraryResources.InsertOneAsync(resource);
        }


        public async Task UpdateAsync(ObjectId id, LibraryResource resource)
        {
            resource.Id = id;
            var result = await _libraryResources.ReplaceOneAsync(r => r.Id == id, resource);
            if (result.MatchedCount == 0)
            {
                throw new Exception("No resource found with the specified ID.");
            }
        }

        public async Task DeleteAsync(ObjectId id) 
        {
            await _libraryResources.DeleteOneAsync(resource => resource.Id == id);
        }

        public async Task<List<LibraryResource>> GetAllAsync()
        {
            return await _libraryResources.Find(_ => true).ToListAsync();
        }
    }
}
