using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Common
{
    public class CreateGradeRequest
    {
        [Required]
        [StringLength(100)]
        public string GradeName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
