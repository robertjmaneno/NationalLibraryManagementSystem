using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Dto
{
    public class MembershipResponseDto
    {
        public string MembershipId { get; set; } = string.Empty; 
        public DateOnly EnrollmentDate { get; set; }
        public string Status { get; set; } = string.Empty; 
        public int OverDueCount { get; set; } 
        public string? PreferredGenres { get; set; } 
    }

}
