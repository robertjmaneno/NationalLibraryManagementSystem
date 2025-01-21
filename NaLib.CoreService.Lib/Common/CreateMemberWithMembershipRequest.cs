using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Common
{
    public class CreateMemberWithMembershipRequest
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? PostalAddress { get; set; }

        public string? PhysicalAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string Status { get; set; } = "Active";

        public string? PreferredGenres { get; set; }
    }

}
