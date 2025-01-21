using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Common
{
    public class LibraryResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsLentOut { get; set; } 
        public bool IsAvailable { get; set; } 
        public bool IsForLending { get; set; } 
    }

}
