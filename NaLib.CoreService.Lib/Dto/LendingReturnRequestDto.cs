using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Dto
{
    public class LendingReturnRequestDto
    {
        public string LibraryResourceId { get; set; }
        public string Condition { get; set; } 
        public int AllowedLendingDays { get; set; }
    }

}
