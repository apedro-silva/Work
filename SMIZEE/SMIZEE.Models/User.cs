using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;

namespace SMIZEE.Models
{
    public class User
    {
        [Key]
        public virtual Guid UserId { get; set; }

        [Required]
        public virtual String Username { get; set; }

        public virtual String Email { get; set; }

        [Required, DataType(DataType.Password)]
        public virtual String Password { get; set; }

        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual String Comment { get; set; }

        public virtual Boolean IsApproved { get; set; }
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }
        public virtual DateTime? LastPasswordFailureDate { get; set; }
        public virtual DateTime? LastActivityDate { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }
        public virtual DateTime? LastLoginDate { get; set; }
        public virtual String ConfirmationToken { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual Boolean IsLockedOut { get; set; }
        public virtual DateTime? LastPasswordChangedDate { get; set; }
        public virtual String PasswordVerificationToken { get; set; }
        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public int? CompanyID { get; set; }

        public int? FunctionalAreaID { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<UserProductionUnit> UserProductionUnit { get; set; }

        public virtual Boolean? IsManager { get; set; }
        public virtual Boolean? IsExecutive { get; set; }
    }

    public static class CxUser
    {
        public static MembershipCreateStatus Register(string Username, string Password, string Email, bool IsApproved, string FirstName, string LastName, int? companyId)
        {
            MembershipCreateStatus CreateStatus;
            MembershipUser user = Membership.CreateUser(Username, Password, Email, null, null, IsApproved, Guid.NewGuid(), out CreateStatus);

            if (CreateStatus == MembershipCreateStatus.Success)
            {
                using (SmizeeContext Context = new SmizeeContext())
                {
                    User User = Context.Users.FirstOrDefault(Usr => Usr.Username == Username);
                    User.CompanyID = companyId;
                    User.FirstName = FirstName;
                    User.LastName = LastName;
                    Context.SaveChanges();
                }
            }

            return CreateStatus;
        }
        public static IQueryable<User> GetUsersByFunctionaAreaId(int functionalAreaId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<User> query = db.Users;
            query = query.Where(p => (p.FunctionalAreaID == functionalAreaId));

            return query;
        }
        public static IQueryable<User> GetManagersByFunctionaAreaId(int functionalAreaId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<User> query = db.Users;
            query = query.Where(p => (p.FunctionalAreaID == functionalAreaId) & (bool)p.IsManager);

            return query;
        }
        public static User GetUserById(Guid userId)
        {
            User user = null;

            using (SmizeeContext Context = new SmizeeContext())
            {
                user = Context.Users.FirstOrDefault(Usr => Usr.UserId == userId);
            }
            return user;

        }
        public static IQueryable<User> GetUsers(int pageNumber, string userName, string firstName, bool? isLocked)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<User> query = db.Users;

            query = query.Where(p => (isLocked==null | p.IsLockedOut==isLocked) & (userName == null | (p.Username == userName)) & (firstName == null | (p.FirstName == firstName)))
                .OrderBy(p => p.Username).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;

        }

    }
}
