using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class FinancialExportForm
    {
        public int FinancialExportFormID { get; set; }

        public int StateID { get; set; }
        public virtual State State { get; set; }

        public int FormID { get; set; }
        public virtual Form Form { get; set; }

        public int ProductionUnitID { get; set; }
        public virtual ProductionUnit ProductionUnit { get; set; }

        public int PeriodNumber { get; set; }

        public DateTime FormDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? UserAlertDate { get; set; }
        public DateTime? ManagerAlertDate { get; set; }
        public DateTime? ExecutiveAlertDate { get; set; }

        public string SubmitUserName { get; set; }
        public string ApprovalUserName { get; set; }

        public virtual ICollection<FinancialExportFormCountry> FinancialExportFormCountries { get; set; }
    }

    public static class CxFinancialExportForm
    {
        public static IQueryable<FinancialExportForm> GetForms(int pageNumber, int? companyId, int? productionUnitId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<FinancialExportForm> query = db.FinancialExportForms;

            query = query.Where(p => (companyId == 0 | (p.ProductionUnit.CompanyID == companyId)) & (productionUnitId == 0 | (p.ProductionUnitID == productionUnitId)))
                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
        }

        public static FinancialExportForm GetFormById(int financialExportFormId)
        {
            FinancialExportForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.FinancialExportForms.FirstOrDefault(f => f.FinancialExportFormID == financialExportFormId);
                
                // force context to get Form
                string code = entity.Form.Periodicity.Code;
                int functionalAreaID = entity.Form.FormType.FunctionalAreaID;
            }
            return entity;
        }

        public static int CreateForm(int formId, int productionUnitId, int periodNumber, DateTime formDate)
        {
            int fid;
            FinancialExportForm entity = new FinancialExportForm();
            using (var db = new Models.SmizeeContext())
            {
                entity.FormID = formId;
                entity.StateID = 1;
                entity.ProductionUnitID = productionUnitId;
                entity.PeriodNumber = periodNumber;
                entity.FormDate = formDate;

                db.FinancialExportForms.Add(entity);
                db.SaveChanges();
                fid = entity.FinancialExportFormID;
            }
            return fid;
        }

        public static IQueryable<FinancialExportForm> GetFormsNotApproved(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<FinancialExportForm> fefInQueue = from f in db.FinancialExportForms
                                                         where f.FormID == formId & f.StateID != 4 select f;
            
            return fefInQueue;
        }
        public static IQueryable<FinancialExportForm> GetFormsByProductionUnit(int formId, int? productionUnitId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<FinancialExportForm> fefInQueue = from fef in db.FinancialExportForms
                                                         where (fef.FormID == formId) & (productionUnitId==null | fef.ProductionUnitID == productionUnitId)
                                                         select fef;

            return fefInQueue;
        }
        public static bool CheckFormsByFormDate(int formId, int productionUnitId, DateTime formDate)
        {
            bool result = false;

            FinancialExportForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.FinancialExportForms.FirstOrDefault(fef => (fef.FormID == formId) & (fef.ProductionUnitID == productionUnitId) & (fef.FormDate == formDate));
                if (entity != null) result = true;
            }
            return result;
        }

        public static IQueryable<FinancialExportForm> GetUserFormsInQueue(SmizeeContext db, string userName, int? companyId, int? productionUnitId, int? functionalAreaId)
        {
            IQueryable<FinancialExportForm> fefInQueue = from f in db.FinancialExportForms
                                                         where (productionUnitId == null | f.ProductionUnitID == productionUnitId)
                                                         & (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                         & (companyId == 0 | f.ProductionUnit.CompanyID == companyId)
                                                         & ((f.SubmitUserName == null & f.StateID == 1)
                                                         | (f.SubmitUserName == userName & (f.StateID == 2 | f.StateID == 5)))
                                                         select f;

            return fefInQueue;
        }

        public static IQueryable<FinancialExportForm> GetFormsForApproval(SmizeeContext db, string userName, int? companyId, int? productionUnitId, int? functionalAreaId)
        {
            IQueryable<FinancialExportForm> fefInQueue = from f in db.FinancialExportForms
                                                         where (productionUnitId == null | f.ProductionUnitID == productionUnitId)
                                                         & (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                         & (companyId == 0 | f.ProductionUnit.CompanyID == companyId)
                                                         & (f.StateID == 3)
                                                         select f;

            return fefInQueue;
        }

        public static void UpdateAlertDate(int financialExportFormId, bool userAlert, bool managerAlert, bool executiveAlert)
        {
            if (!userAlert && !managerAlert && !executiveAlert)
                return;

            // Query the database for the row to be updated.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.FinancialExportForms where c.FinancialExportFormID == financialExportFormId select c).First();
                if (entity != null)
                {
                    if (userAlert)
                        entity.UserAlertDate = DateTime.Now;
                    if (managerAlert)
                        entity.ManagerAlertDate = DateTime.Now;
                    if (executiveAlert)
                        entity.ExecutiveAlertDate = DateTime.Now;

                    db.SaveChanges();
                }
            }
        }

    }

}