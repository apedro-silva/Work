using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using SMIZEE.Models;

namespace SMIZEE.CodeFirstMembership
{

    public sealed class WebSecurity
    {
        public static HttpContextBase Context
        {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        public static HttpRequestBase Request
        {
            get { return Context.Request; }
        }

        public static HttpResponseBase Response
        {
            get { return Context.Response; }
        }

        public static System.Security.Principal.IPrincipal User
        {
            get { return Context.User; }
        }

        public static bool IsAuthenticated
        {
            get { return User.Identity.IsAuthenticated; }
        }

        public static MembershipCreateStatus Register(string Username, string Password, string Email, bool IsApproved, string FirstName, string LastName, int? companyId, int? functionalAreaId, Boolean? isManager, Boolean? isExecutive)
        {
            MembershipCreateStatus CreateStatus;
            MembershipUser user = Membership.CreateUser(Username, Password, Email, null, null, IsApproved, Guid.NewGuid(), out CreateStatus);

            if (CreateStatus == MembershipCreateStatus.Success)
            {
                using (SmizeeContext Context = new SmizeeContext())
                {
                    User User = Context.Users.FirstOrDefault(Usr => Usr.Username == Username);
                    User.CompanyID = companyId;
                    User.FunctionalAreaID = functionalAreaId;
                    User.FirstName = FirstName;
                    User.LastName = LastName;
                    User.IsManager = isManager;
                    User.IsExecutive = isExecutive;
                    Context.SaveChanges();
                }
            }

            return CreateStatus;
        }

        public static Boolean Login(string Username, string Password, bool persistCookie = false)
        {

            bool success = Membership.ValidateUser(Username, Password);
            if (success)
            {
                FormsAuthentication.SetAuthCookie(Username, persistCookie);
            }
            return success;

        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }

        public static MembershipUser GetUser(string Username)
        {
            return Membership.GetUser(Username);
        }

        public static bool LockUser(string userName, bool isLockedOut)
        {
            using (SmizeeContext Context = new SmizeeContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                if (User != null)
                {
                    User.IsLockedOut = isLockedOut;
                    if (isLockedOut)
                        User.LastLockoutDate = DateTime.Now;

                    Context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            bool success = false;
            try
            {
                MembershipUser currentUser = Membership.GetUser(userName, true);
                success = currentUser.ChangePassword(currentPassword, newPassword);
            }
            catch (ArgumentException)
            {

            }

            return success;
        }
        public static string ResetPassword(string userName)
        {
            string newPwd = null;
            try
            {
                MembershipUser currentUser = Membership.GetUser(userName, true);
                newPwd = currentUser.ResetPassword();
            }
            catch (ArgumentException)
            {

            }

            return newPwd;
        }


        public static bool DeleteUser(string Username)
        {
            return Membership.DeleteUser(Username);
        }

        public static Guid GetUserId(string userName)
        {
            MembershipUser user = Membership.GetUser(userName);
            return (Guid)user.ProviderUserKey;
        }

        public static string CreateAccount(string userName, string password)
        {
            return CreateAccount(userName, password, requireConfirmationToken: false);
        }

        public static string CreateAccount(string userName, string password, bool requireConfirmationToken = false)
        {
            CodeFirstMembershipProvider CodeFirstMembership = Membership.Provider as CodeFirstMembershipProvider;
            return CodeFirstMembership.CreateAccount(userName, password, requireConfirmationToken);
        }

        public static string CreateUserAndAccount(string userName, string password)
        {
            return CreateUserAndAccount(userName, password, propertyValues: null, requireConfirmationToken: false);
        }

        public static string CreateUserAndAccount(string userName, string password, bool requireConfirmation)
        {
            return CreateUserAndAccount(userName, password, propertyValues: null, requireConfirmationToken: requireConfirmation);
        }

        public static string CreateUserAndAccount(string userName, string password, IDictionary<string, object> values)
        {
            return CreateUserAndAccount(userName, password, propertyValues: values, requireConfirmationToken: false);
        }

        public static string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
        {
            CodeFirstMembershipProvider CodeFirstMembership = Membership.Provider as CodeFirstMembershipProvider;

            IDictionary<string, object> values = null;
            if (propertyValues != null)
            {
                values = new RouteValueDictionary(propertyValues);
            }

            return CodeFirstMembership.CreateUserAndAccount(userName, password, requireConfirmationToken, values);
        }

        public static List<MembershipUser> FindUsersByEmail(string Email, int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.FindUsersByEmail(Email, PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> FindUsersByName(string Username, int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.FindUsersByName(Username, PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> GetAllUsers(int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.GetAllUsers(PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static void InitializeDatabaseConnection(string connectionStringName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables)
        {

        }

        public static void InitializeDatabaseConnection(string connectionString, string providerName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables)
        {

        }

        public static Guid? GetUserRole(string userName)
        {
            Guid? userRole=null;

            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            using (SmizeeContext Context = new SmizeeContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                if (User != null)
                    userRole = User.Roles.Select(Rl => Rl.RoleId).ToArray()[0];
            }
            return userRole;
        }

        public static void GetUserRoleName(string userName, out Guid? roleId, out string roleName)
        {
            roleId = null;
            roleName = null;

            if (string.IsNullOrEmpty(userName))
            {
                return;
            }
            using (SmizeeContext Context = new SmizeeContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.Username == userName);
                if (User != null)
                {
                    var x = User.Roles.Select(Rl => Rl.RoleId);

                    if (x.Count() > 0)
                    {
                        roleId = x.ToArray()[0];
                        roleName = User.Roles.Select(Rl => Rl.RoleName).ToArray()[0];
                    }
                }
            }
        }

        public static User GetUserInfo(string userName)
        {
            User user = null;

            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            using (SmizeeContext Context = new SmizeeContext())
            {
                user = Context.Users.FirstOrDefault(Usr => Usr.Username == userName);
            }
            return user;

        }

    }
}
