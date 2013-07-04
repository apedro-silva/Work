using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class HumanResourceSDZEEForm
    {
        public int HumanResourceSDZEEFormID { get; set; }

        public int StateID { get; set; }
        public virtual State State { get; set; }

        public int FormID { get; set; }
        public virtual Form Form { get; set; }

        public int PeriodNumber { get; set; }

        [Display(Name = "Nº total de trabalhadores activos da SDZEE")]
        public int? ActiveEmployees { get; set; }

        [Display(Name = "Nº de trabalhadores que sairam da SDZEE")]
        public int? EmployeesLeft { get; set; }

        [Display(Name = "Nº de trabalhadores admitidos na SDZEE")]
        public int? EmployeesHired { get; set; }

        [Display(Name = "Nº Total de horas de formação")]
        public int? TotalTrainingHours { get; set; }

        public DateTime FormDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? UserAlertDate { get; set; }
        public DateTime? ManagerAlertDate { get; set; }
        public DateTime? ExecutiveAlertDate { get; set; }

        public string SubmitUserName { get; set; }

        public string ApprovalUserName { get; set; }

    }
    public static class CxHumanResourceSDZEEForm
    {
        public static HumanResourceSDZEEForm GetFormById(int operationalLicensesFormId)
        {
            HumanResourceSDZEEForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.HumanResourceSDZEEForms.FirstOrDefault(f => f.HumanResourceSDZEEFormID == operationalLicensesFormId);
                // force context to get Form
                string code = entity.Form.Periodicity.Code;
                int functionalAreaID = entity.Form.FormType.FunctionalAreaID;
            }
            return entity;
        }
        public static IQueryable<HumanResourceSDZEEForm> GetForms(int pageNumber, int? year, int? periodicity, int? functionalAreaId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<HumanResourceSDZEEForm> query = db.HumanResourceSDZEEForms;

            query = query.Where(p => (functionalAreaId == 0 | p.Form.FormType.FunctionalAreaID == functionalAreaId) &
                                ((year == null | p.FormDate.Year == year) & (periodicity == 0 | (p.Form.PeriodicityID == periodicity))))
                                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
        }

        public static int CreateForm(int formId, int productionUnitId, int periodNumber, DateTime formDate)
        {
            int HumanResourceFormId = 0;
            HumanResourceSDZEEForm entity = new HumanResourceSDZEEForm();
            using (var db = new Models.SmizeeContext())
            {
                entity.FormID = formId;
                entity.StateID = 1;
                entity.PeriodNumber = periodNumber;
                entity.FormDate = formDate;

                db.HumanResourceSDZEEForms.Add(entity);
                db.SaveChanges();
                HumanResourceFormId = entity.HumanResourceSDZEEFormID;
            }
            return HumanResourceFormId;
        }
        public static IQueryable<HumanResourceSDZEEForm> GetFormsNotApproved(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<HumanResourceSDZEEForm> fInQueue = from f in db.HumanResourceSDZEEForms
                                                     where f.FormID == formId & f.StateID != 4
                                                     select f;

            return fInQueue;
        }
        public static IQueryable<HumanResourceSDZEEForm> GetFormsInQueue(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<HumanResourceSDZEEForm> fInQueue = from fef in db.HumanResourceSDZEEForms
                                                      where (fef.FormID == formId)
                                                      select fef;

            return fInQueue;
        }

        public static IQueryable<HumanResourceSDZEEForm> GetUserFormsInQueue(SmizeeContext db, string userName, int? functionalAreaId)
        {
            //var db = new Models.SmizeeContext();
            IQueryable<HumanResourceSDZEEForm> fefInQueue = from f in db.HumanResourceSDZEEForms
                                                     where (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                     & ((f.SubmitUserName == null & f.StateID == 1)
                                                     | (f.SubmitUserName == userName & (f.StateID == 2 | f.StateID == 5)))
                                                    select f;

            return fefInQueue;
        }
        public static bool CheckFormsByFormDate(int formId, int productionUnitId, DateTime formDate)
        {
            bool result = false;

            HumanResourceSDZEEForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.HumanResourceSDZEEForms.FirstOrDefault(fef => (fef.FormID == formId) & (fef.FormDate == formDate));
                if (entity != null) result = true;
            }
            return result;
        }

    }

}