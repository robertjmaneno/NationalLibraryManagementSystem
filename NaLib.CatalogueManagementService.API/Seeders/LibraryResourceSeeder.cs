using NaLib.CatalogueManagementService.API.Services;
using NaLib.CatalogueManagementService.Lib.Data;
using NaLib.CatalogueManagementService.Lib.Dto;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaLib.CatalogueManagementService.API.Seeders
{
    public class LibraryResourceSeeder
    {
        private readonly LibraryResourceService _service;
        private readonly IMapper _mapper;

        public LibraryResourceSeeder(LibraryResourceService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task SeedAsync()
        {
            var existingResources = await _service.GetAllAsync();
            if (existingResources.Any()) return;

            var resourcesDto = new List<CreateLibraryResourceDto>
            {
                new CreateLibraryResourceDto
                {
                    Title = "The Great Gatsby",
                    ResourceType = "Book",
                    Format = "Hard Copy",
                    Genres = new List<string> { "Fiction", "Classic" },
                    Authors = new List<string> { "Fiction", "Classic" },
                    Publishers = new List<string> { "Fiction", "Classic" },
                    CatalogedBy = 1 
                },
                new CreateLibraryResourceDto
                {
                    Title = "National Daily Newspaper",
                    ResourceType = "Newspaper",
                    Format = "Electronic Copy",
                    Genres = new List<string> { "News", "Current Affairs" },
                     Authors = new List<string> { "Fiction", "Classic" },
                    Publishers = new List<string> { "Fiction", "Classic" },
                    CatalogedBy = 2 
                },
                new CreateLibraryResourceDto
                {
                    Title = "Scientific American Article: Quantum Physics",
                    ResourceType = "Article",
                    Format = "Electronic Copy",
                    Genres = new List<string> { "Science", "Education" },
                     Authors = new List<string> { "Fiction", "Classic" },
                    Publishers = new List<string> { "Fiction", "Classic" },
                    CatalogedBy = 3
                }
            };


            foreach (var resourceDto in resourcesDto)
            {

                var resource = _mapper.Map<LibraryResource>(resourceDto);

  
                if (resource.ResourceType == "Book")
                {
                    resource.IsBorrowable = true;
                    resource.BorrowRules = new BorrowRule
                    {
                        BorrowLimitInDays = 10
                    };
                }
                else
                {
                    resource.IsBorrowable = false;
                    resource.BorrowRules = null; 
                }

                
                resource.BorrowStatus = BorrowStatus.Available;


                await _service.CreateAsync(resource);
            }
        }
    }
}
