using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class OperationalLicensesForm
    {
        public int OperationalLicensesFormID { get; set; }

        public int StateID { get; set; }
        public virtual State State { get; set; }

        public int FormID { get; set; }
        public virtual Form Form { get; set; }

        public int PeriodNumber { get; set; }

        public DateTime FormDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? UserAlertDate { get; set; }
        public DateTime? ManagerAlertDate { get; set; }
        public DateTime? ExecutiveAlertDate { get; set; }

        public string SubmitUserName { get; set; }

        public string ApprovalUserName { get; set; }
    }
    public static class CxOperationalLicensesForm
    {
        public static void UpdateRecord(int operationalLicensesFormID, string userName, int formState)
        {
            // remove all country export records associated to the current formId
            CxOperationalLicensesFormPhase.Delete(operationalLicensesFormID);

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.OperationalLicensesForms where c.OperationalLicensesFormID == operationalLicensesFormID select c).First();
                if (entity != null)
                {
                    entity.StateID = formState;
                    if (formState <= 3)
                    {
                        entity.SubmitDate = DateTime.Now;
                        entity.SubmitUserName = userName;
                    }
                    else
                    {
                        entity.ApprovalUserName = userName;
                        entity.ApprovalDate = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }
        }
        public static void CreateFormPhaseRecord(int operationalLicensesFormID, string userName, int formState, int devPhase, int PUN)
        {
            OperationalLicensesFormPhase olfp;

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                olfp = new OperationalLicensesFormPhase();
                olfp.DevelopmentPhaseID = devPhase;
                olfp.ProductionUnitsNumber = PUN;
                olfp.OperationalLicensesFormID = operationalLicensesFormID;
                db.OperationalLicensesFormPhases.Add(olfp);
                db.SaveChanges();
            }
        }

        public static void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from OperationalLicensesForms where OperationalLicensesFormID={0}", code);
                var entity = (from c in db.OperationalLicensesForms where c.OperationalLicensesFormID == code select c).First();
                if (entity != null)
                {
                    db.OperationalLicensesForms.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        public static void CreateRecord(int formId, string userName)
        {
            using (var db = new Models.SmizeeContext())
            {
                OperationalLicensesForm entity = new OperationalLicensesForm();
                entity.StateID = 1;
                entity.FormID = formId;
                entity.SubmitDate = DateTime.Now;
                entity.SubmitUserName = userName;

                db.OperationalLicensesForms.Add(entity);
                db.SaveChanges();
            }
        }


        public static OperationalLicensesForm GetFormById(int operationalLicensesFormId)
        {
            OperationalLicensesForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.OperationalLicensesForms.FirstOrDefault(f => f.OperationalLicensesFormID == operationalLicensesFormId);
                // force context to get Form
                string code = entity.Form.Periodicity.Code;
                int functionalAreaID = entity.Form.FormType.FunctionalAreaID;
            }
            return entity;
        }

        public static int CreateForm(int formId, int productionUnitId, int periodNumber, DateTime formDate)
        {
            int OperationalLicensesFormId = 0;
            OperationalLicensesForm entity = new OperationalLicensesForm();
            using (var db = new Models.SmizeeContext())
            {
                entity.FormID = formId;
                entity.StateID = 1;
                entity.PeriodNumber = periodNumber;
                entity.FormDate = formDate;

                db.OperationalLicensesForms.Add(entity);
                db.SaveChanges();
                OperationalLicensesFormId = entity.OperationalLicensesFormID;
            }
            return OperationalLicensesFormId;
        }
        public static IQueryable<OperationalLicensesForm> GetFormsNotApproved(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<OperationalLicensesForm> fInQueue = from f in db.OperationalLicensesForms
                                                           where f.FormID == formId & f.StateID != 4
                                                           select f;

            return fInQueue;
        }
        public static IQueryable<OperationalLicensesForm> GetFormsByProductionUnit(int formId, int productionUnitId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<OperationalLicensesForm> fInQueue = from fef in db.OperationalLicensesForms
                                                   where (fef.FormID == formId)
                                                   select fef;
            return fInQueue;
        }

        public static bool CheckFormsByFormDate(int formId, int productionUnitId, DateTime formDate)
        {
            bool result = false;

            OperationalLicensesForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.OperationalLicensesForms.FirstOrDefault(fef => (fef.FormID == formId) & (fef.FormDate == formDate));
                if (entity != null) result = true;
            }
            return result;
        }

        public static IQueryable<OperationalLicensesForm> GetForms(int pageNumber, int? year, int? periodicity, int? functionalAreaId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<OperationalLicensesForm> query = db.OperationalLicensesForms;

            query = query.Where(p => (functionalAreaId == 0 | p.Form.FormType.FunctionalAreaID == functionalAreaId) &
                                ((year == null | p.FormDate.Year == year) & (periodicity == 0 | (p.Form.PeriodicityID == periodicity))))
                                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);


            return query;
        }
        public static IQueryable<OperationalLicensesForm> GetUserFormsInQueue(SmizeeContext db, string userName, int? functionalAreaId)
        {
            //var db = new Models.SmizeeContext();
            IQueryable<OperationalLicensesForm> fefInQueue = from f in db.OperationalLicensesForms
                                                     where (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                     & ((f.SubmitUserName == null & f.StateID == 1)
                                                     | (f.SubmitUserName == userName & (f.StateID == 2 | f.StateID == 5)))
                                                     select f;

            return fefInQueue;
        }

        public static void UpdateAlertDate(int operationalLicencesFormId, bool userAlert, bool managerAlert, bool executiveAlert)
        {
            if (!userAlert && !managerAlert && !executiveAlert)
                return;

            // Query the database for the row to be updated.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.OperationalLicensesForms where c.OperationalLicensesFormID == operationalLicencesFormId select c).First();
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