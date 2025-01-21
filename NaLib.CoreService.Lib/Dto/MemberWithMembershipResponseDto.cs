using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Dto
{
    public class MemberWithMembershipResponseDto
    {
        public int MemberId { get; set; } 
        public string FirstName { get; set; } = string.Empty; 
        public string LastName { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty; 
        public string? PhoneNumber { get; set; } 
        public string? PostalAddress { get; set; } 
        public string? PhysicalAddress { get; set; } 
        public DateOnly CreatedAt { get; set; } 

        public MembershipResponseDto Membership { get; set; } = new MembershipResponseDto(); 
    }

}
