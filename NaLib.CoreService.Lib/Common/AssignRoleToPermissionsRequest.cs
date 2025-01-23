using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Common
{
    public class AssignRoleToPermissionsRequest
    {
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string RoleName { get; set; }

        [Required]
        public List<string> PermissionNames { get; set; }
    }
}
