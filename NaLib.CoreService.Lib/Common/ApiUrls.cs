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
        public const string StaffDetails = BaseUrl + "/StaffDetails"; 

        public const string Staff = BaseUrl + "/Staff";
        public const string PermissionsAndRoles = BaseUrl + "/RolesAndPermissions";
        public const string login = BaseUrl + "/Login";
        public const string UserDetails = BaseUrl + "UserDetails";

        public const string CreateMember = "createMember";
        public const string GetMember = "getMember";
        public const string UpdateMember= "updateMember";
        public const string DeleteMember = "deleteMember";
        public const string GetAllMembers = "getAllMembers";


        public const string Permissions = "Permissions";
        public const string CreatePermission = "createPermission";
        public const string GetPermission = "getPermission";
        public const string GetPermissionID = "getPerissinID";
        public const string UpdatePermission = "updatePermission";


        public const string CreateRole = "createRole";
        public const string GetRole = "getRole";
        public const string UpdateRole= "updateRole";
        public const string GetAllRoles = "getAllRoles";
        public const string AssignRoleToPermissions = "AssignRoleToPermissions";


        public const string CreateSkills = "createSkills";
        public const string CreateQualifications = "createQualifications";
        public const string CreateGrades = "createGrades";
        public const string CreateExperiences = "createExperience";
        public const string AssignSkillToUser = "assignSkillToUser";



        public const string LendingTransaction = BaseUrl + "/LendingTransaction";
        public const string CheckoutResources = "checkoutResource";
        public const string ReturnResources = "returnResource";




        public const string CreateStaff = "createStaff";
        public const string GetStaff = "getStaff";
        public const string GetAllStaffs = "getAllStaffs";








        public const string UserLogin = "useLogin";

    }
}
