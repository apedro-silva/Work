using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class HumanResourceForm
    {
        public int HumanResourceFormID { get; set; }

        public int StateID { get; set; }
        public virtual State State { get; set; }

        public int FormID { get; set; }
        public virtual Form Form { get; set; }

        public int ProductionUnitID { get; set; }
        public virtual ProductionUnit ProductionUnit { get; set; }

        public int PeriodNumber { get; set; }

        [Display(Name = "Nº total de horas de formação")]
        public int? TrainingTotalHours { get; set; }

        [Display(Name = "Nº de dias de ausência ao trabalho")]
        public int? WorkMissingDays { get; set; }

        [Display(Name = "Nº de dias úteis do período")]
        public int? WorkingDaysInPeriod { get; set; }

        [Display(Name = "Nº colaboradores admitidos")]
        public int? EmployeesHired { get; set; }

        [Display(Name = "Nº de saídas")]
        public int? EmployeesLeft { get; set; }

        public DateTime FormDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? UserAlertDate { get; set; }
        public DateTime? ManagerAlertDate { get; set; }
        public DateTime? ExecutiveAlertDate { get; set; }

        public string SubmitUserName { get; set; }

        public string ApprovalUserName { get; set; }

    }
    public static class CxHumanResourceForm
    {
        public static HumanResourceForm GetFormById(int operationalLicensesFormId)
        {
            HumanResourceForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.HumanResourceForms.FirstOrDefault(f => f.HumanResourceFormID == operationalLicensesFormId);
                // force context to get Form
                string code = entity.Form.Periodicity.Code;
                int functionalAreaID = entity.Form.FormType.FunctionalAreaID;
            }
            return entity;
        }
        public static IQueryable<HumanResourceForm> GetForms(int pageNumber, int? companyId, int? productionUnitId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<HumanResourceForm> query = db.HumanResourceForms;

            query = query.Where(p => (companyId == 0 | (p.ProductionUnit.CompanyID == companyId)) & (productionUnitId == 0 | (p.ProductionUnitID == productionUnitId)))
                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
        }

        public static int CreateForm(int formId, int productionUnitId, int periodNumber, DateTime formDate)
        {
            int HumanResourceFormId = 0;
            HumanResourceForm entity = new HumanResourceForm();
            using (var db = new Models.SmizeeContext())
            {
                entity.FormID = formId;
                entity.StateID = 1;
                entity.ProductionUnitID = productionUnitId;
                entity.PeriodNumber = periodNumber;
                entity.FormDate = formDate;

                db.HumanResourceForms.Add(entity);
                db.SaveChanges();
                HumanResourceFormId = entity.HumanResourceFormID;
            }
            return HumanResourceFormId;
        }
        public static IQueryable<HumanResourceForm> GetFormsNotApproved(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<HumanResourceForm> fInQueue = from f in db.HumanResourceForms
                                                     where f.FormID == formId & f.StateID != 4
                                                     select f;

            return fInQueue;
        }
        public static IQueryable<HumanResourceForm> GetFormsByProductionUnit(int formId, int productionUnitId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<HumanResourceForm> fInQueue = from fef in db.HumanResourceForms
                                                     where (fef.FormID == formId) & (fef.ProductionUnitID == productionUnitId)
                                                     select fef;

            return fInQueue;
        }
        public static bool CheckFormsByFormDate(int formId, int productionUnitId, DateTime formDate)
        {
            bool result = false;

            HumanResourceForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.HumanResourceForms.FirstOrDefault(fef => (fef.FormID == formId) & (fef.ProductionUnitID == productionUnitId) & (fef.FormDate == formDate));
                if (entity != null) result = true;
            }
            return result;
        }

        public static IQueryable<HumanResourceForm> GetUserFormsInQueue(SmizeeContext db, string userName, int? companyId, int? productionUnitId, int? functionalAreaId)
        {
            //var db = new Models.SmizeeContext();
            IQueryable<HumanResourceForm> fefInQueue = from f in db.HumanResourceForms
                                                     where (productionUnitId == null | f.ProductionUnitID == productionUnitId)
                                                     & (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                     & (companyId == 0 | f.ProductionUnit.CompanyID == companyId)
                                                     & ((f.SubmitUserName == null & f.StateID == 1)
                                                     | (f.SubmitUserName == userName & (f.StateID == 2 | f.StateID == 5)))
                                                     select f;

            return fefInQueue;
        }
        public static IQueryable<HumanResourceForm> GetFormsForApproval(SmizeeContext db, string userName, int? companyId, int? productionUnitId, int? functionalAreaId)
        {
            //var db = new Models.SmizeeContext();
            IQueryable<HumanResourceForm> fefInQueue = from f in db.HumanResourceForms
                                                       where (productionUnitId == null | f.ProductionUnitID == productionUnitId)
                                                       & (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                       & (companyId == 0 | f.ProductionUnit.CompanyID == companyId)
                                                       & (f.StateID == 3)
                                                       select f;

            return fefInQueue;
        }


        public static void UpdateAlertDate(int humanResourceFormID, bool userAlert, bool managerAlert, bool executiveAlert)
        {
            if (!userAlert && !managerAlert && !executiveAlert)
                return;

            // Query the database for the row to be updated.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.HumanResourceForms where c.HumanResourceFormID == humanResourceFormID select c).First();
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