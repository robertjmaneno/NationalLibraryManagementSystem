using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Common
{
    public class CreateSkillRequest
    {
        [Required]
        [StringLength(255)]
        public string SkillName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
