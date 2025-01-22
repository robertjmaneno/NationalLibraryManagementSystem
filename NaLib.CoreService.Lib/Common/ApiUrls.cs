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

        public const string Staff = BaseUrl + "/Staff";
        public const string Roles = BaseUrl + "/Roles_And_Permissions";
        public const string login = BaseUrl + "/Login";

        public const string CreateMember = "createMember";
        public const string GetMember = "getMember";
        public const string UpdateMember= "updateMember";
        public const string DeleteMember = "deleteMember";
        public const string GetAllMembers = "getAllMembers";

        // Lending Transaction Endpoints
        public const string LendingTransaction = BaseUrl + "/LendingTransaction";
        public const string CheckoutResources = "checkout";
        public const string ReturnResources = "return";


        public const string CreateStaff = "createStaff";
        public const string GetStaff = "getSatff";

        public const string CreateRole = "createRole";
        public const string GetRole = "getRole";


        public const string Permissions = "Permissions";
        public const string CreatePermission = "createPermission";
        public const string GetPermission = "getPermission";
        public const string GetPermissionID = "getPerissinID";
        public const string UpdatePermission = "updatePermission";


        public const string CreateRolePermission = "createRolePermission";
        public const string GetRolePermission = "getRolePermission";
        public const string GetRolePermissionID = "getRolePermissionById";

        public const string UserLogin = "useLogin";

    }
}
