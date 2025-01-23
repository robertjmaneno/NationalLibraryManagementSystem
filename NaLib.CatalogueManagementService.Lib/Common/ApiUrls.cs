using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CatalogueManagementService.Lib.Common
{
    public class ApiUrls
    {
        
        public const string BaseUrl = "api/v1";
        public const string LibraryResource = BaseUrl + "/LibraryResource";

        public const string createLibraryResource = "createLibraryResource";
        public const string updateLibraryResource = "updateLibraryResource";
        public const string getLibraryResource = "getLibraryResource";
        public const string getAllLibraryResources = "getAllLibraryResources";
        public const string deleteLibraryResource = "deleteLibraryResource";
    }
}
