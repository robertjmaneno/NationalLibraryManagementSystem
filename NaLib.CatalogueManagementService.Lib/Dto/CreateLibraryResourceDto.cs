using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CatalogueManagementService.Lib.Dto
{
    public class CreateLibraryResourceDto
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        public string ResourceType { get; set; }

        [Required]
        public string Format { get; set; } 

        public List<string> Genres { get; set; } = new List<string>();

        public List<string> Authors { get; set; } = new List<string>();

        public List<string> Publishers { get; set; } = new List<string>();

        [Required]
        public int CatalogedBy { get; set; }

        [Required]
        public string BorrowStatus { get; set; }

    }
}
