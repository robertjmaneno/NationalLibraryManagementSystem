using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Common
{
    public class CreateExperienceRequest
    {
        public int YearsOfExperience { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
