using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Dto
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public RoleDto Role { get; set; }
        public ICollection<QualificationDto> Qualifications { get; set; }
        public ICollection<ExperienceDto> Experiences { get; set; }
        public ICollection<SkillDto> UserSkills
        {
            get; set;
        }
    }
}