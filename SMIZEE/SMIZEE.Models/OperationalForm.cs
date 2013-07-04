using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class OperationalForm
    {
        public int OperationalFormID { get; set; }

        public int StateID { get; set; }
        public virtual State State { get; set; }

        public int FormID { get; set; }
        public virtual Form Form { get; set; }

        public int PeriodNumber { get; set; }

        [Display(Name = "Used Space")]
        public int? UsedSpace { get; set; }

        [Display(Name = "Quadrant 1 Hectares Number")]
        public int? Quadrant1HectaresNumber { get; set; }

        [Display(Name = "Quadrant 2 Hectares Number")]
        public int? Quadrant2HectaresNumber { get; set; }
        
        [Display(Name = "Quadrant 3 Hectares Number")]
        public int? Quadrant3HectaresNumber { get; set; }

        [Display(Name = "Quadrant 4 Hectares Number")]
        public int? Quadrant4HectaresNumber { get; set; }

        [Display(Name = "Production Units Total Number")]
        public int? ProductionUnitsTotalNumber { get; set; }

        public DateTime FormDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? UserAlertDate { get; set; }
        public DateTime? ManagerAlertDate { get; set; }
        public DateTime? ExecutiveAlertDate { get; set; }

        public string SubmitUserName { get; set; }
        public string ApprovalUserName { get; set; }
    }
    public static class CxOperationalForm
    {
        public static void UpdateRecord(int operationalFormId, int formState, string userName, int? numeroHectaresQuadrante1, int? numeroHectaresQuadrante2,
                                int? numeroHectaresQuadrante3, int? numeroHectaresQuadrante4, int? numeroHectaresEspacoUtilizado,
                                int? numeroTotalUnidadesProdutivas)
        {
            //Query the database for the rows to be updated.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.OperationalForms where c.OperationalFormID == operationalFormId select c).First();
                if (entity != null)
                {
                    entity.OperationalFormID = operationalFormId;
                    entity.ProductionUnitsTotalNumber = numeroTotalUnidadesProdutivas;
                    entity.Quadrant1HectaresNumber = numeroHectaresQuadrante1;
                    entity.Quadrant2HectaresNumber = numeroHectaresQuadrante2;
                    entity.Quadrant3HectaresNumber = numeroHectaresQuadrante3;
                    entity.Quadrant4HectaresNumber = numeroHectaresQuadrante4;

                    entity.UsedSpace = numeroHectaresEspacoUtilizado;
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

        public static void DeleteRecord(int code)
        {
            //Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from OperationalForms where OperationalFormId={0}", code);
                var entity = (from c in db.OperationalForms where c.OperationalFormID == code select c).First();
                if (entity != null)
                {
                    db.OperationalForms.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        public static void CreateRecord(int formId, string userName, int? numeroHectaresQuadrante1, int? numeroHectaresQuadrante2,
                                int? numeroHectaresQuadrante3, int? numeroHectaresQuadrante4, int? numeroHectaresEspacoUtilizado,
                                int? numeroTotalUnidadesProdutivas)
        {
            using (var db = new Models.SmizeeContext())
            {
                OperationalForm entity = new OperationalForm();
                entity.StateID = 1;
                entity.FormID = formId;
                entity.UsedSpace = numeroHectaresEspacoUtilizado;
                entity.ProductionUnitsTotalNumber = numeroTotalUnidadesProdutivas;
                entity.Quadrant1HectaresNumber = numeroHectaresQuadrante1;
                entity.Quadrant2HectaresNumber = numeroHectaresQuadrante2;
                entity.Quadrant3HectaresNumber = numeroHectaresQuadrante3;
                entity.Quadrant4HectaresNumber = numeroHectaresQuadrante4;

                entity.SubmitDate = DateTime.Now;
                entity.SubmitUserName = userName;

                db.OperationalForms.Add(entity);
                db.SaveChanges();
            }
        }

        public static OperationalForm GetFormById(int operationalFormId)
        {
            OperationalForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.OperationalForms.FirstOrDefault(f => f.OperationalFormID == operationalFormId);
                // force context to get Form
                string code = entity.Form.Periodicity.Code;
                int functionalAreaID = entity.Form.FormType.FunctionalAreaID;
            }
            return entity;
        }
        public static IQueryable<OperationalForm> GetForms(int pageNumber, int? year, int? periodicity, int? functionalAreaId)
        {
            int numberOfObjectsPerPage = 10;
            var db = new Models.SmizeeContext();
            IQueryable<OperationalForm> query = db.OperationalForms;

            query = query.Where(p => (functionalAreaId == 0 | p.Form.FormType.FunctionalAreaID == functionalAreaId) & 
                                ((year == null | p.FormDate.Year == year) | (periodicity == 0 | (p.Form.PeriodicityID == periodicity))))
                                .OrderBy(p => p.FormID).Skip(numberOfObjectsPerPage * (pageNumber - 1)).Take(numberOfObjectsPerPage + 1);

            return query;
        }

        public static int CreateForm(int formId, int productionUnitId, int periodNumber, DateTime formDate)
        {
            int OperationalFormId = 0;
            OperationalForm entity = new OperationalForm();
            using (var db = new Models.SmizeeContext())
            {
                entity.FormID = formId;
                entity.StateID = 1;
                entity.PeriodNumber = periodNumber;
                entity.FormDate = formDate;

                db.OperationalForms.Add(entity);
                db.SaveChanges();
                OperationalFormId = entity.OperationalFormID;
            }
            return OperationalFormId;
        }
        public static IQueryable<OperationalForm> GetFormsNotApproved(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<OperationalForm> fInQueue = from f in db.OperationalForms
                                                   where f.FormID == formId & f.StateID != 4
                                                   select f;

            return fInQueue;
        }
        public static IQueryable<OperationalForm> GetFormsByProductionUnit(int formId, int productionUnitId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<OperationalForm> fInQueue = from fef in db.OperationalForms
                                                   where (fef.FormID == formId) 
                                                   select fef;
            return fInQueue;
        }
        public static bool CheckFormsByFormDate(int formId, int productionUnitId, DateTime formDate)
        {
            bool result = false;

            OperationalForm entity = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                entity = Context.OperationalForms.FirstOrDefault(fef => (fef.FormID == formId) & (fef.FormDate == formDate));
                if (entity != null) result = true;
            }
            return result;
        }

        public static IQueryable<OperationalForm> GetFormsInQueue(int formId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<OperationalForm> fInQueue = from fef in db.OperationalForms
                                                      where (fef.FormID == formId)
                                                      select fef;

            return fInQueue;
        }
        public static IQueryable<OperationalForm> GetUserFormsInQueue(SmizeeContext db, string userName, int? functionalAreaId)
        {
            //var db = new Models.SmizeeContext();
            IQueryable<OperationalForm> fefInQueue = from f in db.OperationalForms
                                                             where (functionalAreaId == null | f.Form.FormType.FunctionalAreaID == functionalAreaId)
                                                             & ((f.SubmitUserName == null & f.StateID == 1)
                                                             | (f.SubmitUserName == userName & (f.StateID == 2 | f.StateID == 5)))
                                                             select f;

            return fefInQueue;
        }

    }

}