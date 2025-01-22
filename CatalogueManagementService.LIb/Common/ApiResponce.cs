using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogueManagementService.LIb.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Remark { get; set; }
        public T? Data { get; set; }
        public List<ApiError> Errors { get; set; } = new();
    }

    public class ApiError
    {
        public int ErrorCode { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
