using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Dto
{
    public class LendingCheckoutRequestDto 
    {
        public string MembershipId { get; set; }
        public string LibraryResourceId { get; set; }
    }

}
