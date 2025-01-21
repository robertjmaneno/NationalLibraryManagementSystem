using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Dto
{
    public class CreateMemberWithMembershipDto
    {

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
        public string? PostalAddress { get; set; }
        public string? PhysicalAddress { get; set; }

        public string? PreferredGenres { get; set; }
        [Required]
        public string Status { get; set; } = "Active";
    }

}
