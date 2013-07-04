using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public static class AuditAction
    {
        public static void Create(string userName, string applicationPage, int operationId, string result)
        {
            using (var db = new SmizeeContext())
            {
                Audit entity = new Audit();
                entity.UserName = userName;
                entity.ApplicationPage = applicationPage;
                entity.OperationID = operationId;
                entity.Timestamp = DateTime.Now;
                entity.Result = result;

                db.Audits.Add(entity);
                db.SaveChanges();
            }

        }
    }

    public class Audit
    {
        [ScaffoldColumn(false)]
        public int AuditID { get; set; }

        public DateTime Timestamp { get; set; }

        [Required, StringLength(100), Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required, StringLength(100), Display(Name = "ApplicationPage")]
        public string ApplicationPage { get; set; }

        [Required]
        public int OperationID { get; set; }
        public virtual Operation Operation { get; set; }

        [Required, StringLength(1000), Display(Name = "Result"), DataType(DataType.MultilineText)]
        public string Result { get; set; }

    }

    public class Service
    {
        [Key]
        public int ServiceID { get; set; }

        [Required, StringLength(100), Display(Name = "ApplicationPage")]
        public string ApplicationPage { get; set; }

        [Required, StringLength(100), Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class Operation
    {
        [Key]
        public int OperationID { get; set; }

        [Required, StringLength(100), Display(Name = "Description")]
        public string Description { get; set; }
    }

    public static class CxAudits
    {
        public static IQueryable<Audit> GetList(SmizeeContext db, int pageNumber, string applicationPage, string userName, DateTime? startTimestamp, DateTime? endTimestamp)
        {
            int numberOfObjectsPerPage = 10;
            IQueryable<Audit> query = db.Audits;

            query = query.Where(p => 
                                (applicationPage == null | (p.ApplicationPage == applicationPage)) & 
                                (startTimestamp == null | (p.Timestamp >= startTimestamp)) &
                                (endTimestamp == null | (p.Timestamp <= endTimestamp)) &
                                (userName==null | (p.UserName==userName)))
                                .OrderByDescending(p => p.AuditID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
        }

        public static IQueryable<Service> GetServices(SmizeeContext db)
        {
            IQueryable<Service> query = db.Services;

            return query;
        }

    }
}