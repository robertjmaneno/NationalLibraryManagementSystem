using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Common
{
    public class Response<T>
    {

        public int StatusCode { get; set; }
        public string? Remark { get; set; }
        public T? Data { get; set; }
        public List<ValidationError> Errors { get; set; } = new();
    }
}
