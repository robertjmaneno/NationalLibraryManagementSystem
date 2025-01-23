using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Common
{
    public class CreatePermissionRequest
    {
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string PermissionsName { get; set; }

        [StringLength(255)]
        [Unicode(false)]
        public string? Description { get; set; }
    }
}
