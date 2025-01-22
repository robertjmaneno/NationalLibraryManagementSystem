using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Common
{
    public class ReturnResourceRequestDto
    {
        public string MembershipId { get; set; }
        public List<ResourceReturnInfo> ResourceReturnInfo { get; set; }
    }
}
