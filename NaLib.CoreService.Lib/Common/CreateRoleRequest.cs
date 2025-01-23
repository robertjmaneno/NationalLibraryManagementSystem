using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Common
{

        public class CreateRoleRequest
        {
            [Required]
            [StringLength(100)]
            [Unicode(false)]
            public string RoleName { get; set; }

            [StringLength(255)]
            [Unicode(false)]
            public string? Description { get; set; }
        }
    }

