using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Common
{
    public class ApiUrls
    {
        
        public const string BaseUrl = "api/v1";
        public const string Member = BaseUrl + "/Member";

        public const string CreateMember = "createMember";
        public const string GetMember = "getMember";
        public const string UpdateMember= "updateMember";
        public const string DeleteMember = "deleteMember";
        public const string GetAllMembers = "getAllMembers";

    }
}
