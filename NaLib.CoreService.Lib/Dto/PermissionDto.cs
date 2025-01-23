using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Dto
{
    public class PermissionDto
    {

        public string PermissionsName { get; set; }
        public string? Description { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
