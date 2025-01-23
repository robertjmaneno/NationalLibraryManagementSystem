using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CatalogueManagementService.Lib.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Remark { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

}
