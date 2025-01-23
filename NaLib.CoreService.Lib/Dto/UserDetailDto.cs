using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Dto
{
    public class UserDetailDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public DateOnly UserCreatedAt { get; set; }
        public string? LibraryName { get; set; }
        public string? RoleName { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? GradeName { get; set; }
        public string? QualificationName { get; set; }
        public string? SkillName { get; set; }
    }

}
