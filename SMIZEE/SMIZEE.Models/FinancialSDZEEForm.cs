using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class FinancialSDZEEForm
    {
        [Key]
        public int FinancialSDZEEFormID { get; set; }

        public int StateID { get; set; }
        public virtual State State { get; set; }

        public int FormID { get; set; }
        public virtual Form Form { get; set; }

        public int PeriodNumber { get; set; }

        [Display(Name = "Sales Net Amount")]
        public decimal? SalesNetAmount { get; set; }

        [Display(Name = "Service Delivery Net Amount")]
        public decimal? ServiceDeliveryNetAmount { get; set; }

        [Display(Name = "Current Assets Value")]
        public decimal? CurrentAssetsValue { get; set; }

        [Display(Name = "value of current liabilities")]
        public decimal? CurrentLiabilitiesValue { get; set; }

        [Display(Name = "Total Cost Value")]
        public decimal? TotalCostValue { get; set; }

        [Display(Name = "Liability Value")]
        public decimal? LiabilityValue { get; set; }

        [Display(Name = "Asset Value")]
        public decimal? AssetValue { get; set; }

        public DateTime FormDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? UserAlertDate { get; set; }
        public DateTime? ManagerAlertDate { get; set; }
        public DateTime? ExecutiveAlertDate { get; set; }

        public string SubmitUserName { get; set; }
        public string ApprovalUserName { get; set; }

    }

    public static class  CxFinancialSDZEEForm
    {
        public static IQueryable<FinancialSDZEEForm> GetForms(int pageNumber, int? year, int? periodicity, int? functionalAreaId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<FinancialSDZEEForm> query = db.FinancialSDZEEForms;

            query = query.Where(p => (functionalAreaId == 0 | p.Form.FormType.FunctionalAreaID == functionalAreaId) &
                                ((year == null | p.FormDate.Year == year) & (periodicity == 0 | (p.Form.PeriodicityID == periodicity))))
                                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
        }

        public static FinancialSDZEEForm GetFormById(int financialFormId)
        {
            FinancialSDZEEForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.FinancialSDZEEForms.FirstOrDefault(f => f.FinancialSDZEEFormID == financialFormId);
                // force context to get Form
                string code = entity.Form.Periodicity.Code;
                int functionalAreaID = entity.Form.FormType.FunctionalAreaID;
            }
            return entity;
        }

        public static int CreateForm(int formId, int productionUnitId, int periodNumber, DateTime formDate)
        {
            int financialFormId = 0;
            FinancialSDZEEForm entity = new FinancialSDZEEForm();
            using (var db = new Models.SmizeeContext())
            {
                entity.FormID = formId;
                entity.StateID = 1;
                entity.PeriodNumber = periodNumber;
                entity.FormDate = formDate;

                db.FinancialSDZEEForms.Add(entity);
                db.SaveChanges();
                financialFormId = entity.FinancialSDZEEFormID;
            }
            return financialFormId;
        }
        public static IQueryable<FinancialSDZEEForm> GetFormsNotApproved(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<FinancialSDZEEForm> fInQueue = from f in db.FinancialSDZEEForms
                                                 where f.FormID == formId & f.StateID != 4
                                                 select f;

            return fInQueue;
        }
        public static IQueryable<FinancialSDZEEForm> GetFormsInQueue(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<FinancialSDZEEForm> fInQueue = from fef in db.FinancialSDZEEForms
                                                 where (fef.FormID == formId)
                                                 select fef;

            return fInQueue;
        }
        public static IQueryable<FinancialSDZEEForm> GetUserFormsInQueue(SmizeeContext db, string userName, int? functionalAreaId)
        {
            IQueryable<FinancialSDZEEForm> fefInQueue = from f in db.FinancialSDZEEForms
                                                         where (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                         & ((f.SubmitUserName == null & f.StateID == 1)
                                                         | (f.SubmitUserName == userName & (f.StateID == 2 | f.StateID == 5)))
                                                         select f;

            return fefInQueue;
        }
        public static bool CheckFormsByFormDate(int formId, int productionUnitId, DateTime formDate)
        {
            bool result = false;

            FinancialSDZEEForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.FinancialSDZEEForms.FirstOrDefault(fef => (fef.FormID == formId) & (fef.FormDate == formDate));
                if (entity != null) result = true;
            }
            return result;
        }

    }
}