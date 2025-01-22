using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogueManagementService.LIb.Common
{
    public class KnowbUrls
    {
        public const string BaseUrl = "api/v1";
        public const string Catalogue = BaseUrl + "/Catalogue";

        public const string CreateResource = "createResource";
        public const string GetResource = "getResource";
        public const string UpdateResource = "updateResource";
        public const string DeleteResource = "deleteResource";
        public const string SearchResources = "searchResource";
        public const string CheckResourceAvailability = "checkResourceAvailability";
    }
}
