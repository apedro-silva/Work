using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class Form
    {
        [Key]
        public int FormID { get; set; }

        [Required, StringLength(50), Display(Name = "Small Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Description")]
        public string Description { get; set; }

        public int FormTypeID { get; set; }
        public virtual FormType FormType { get; set; }

        public int PeriodicityID { get; set; }
        public virtual Periodicity Periodicity { get; set; }

        public virtual int? AlarmRangeUser { get; set; }
        public virtual int? AlarmRangeManager { get; set; }
        public virtual int? AlarmRangeExecutive { get; set; }
    }

    public static class FormStart
    {
        public static List<Form> GetForms()
        {
            var forms = new List<Form> {
                new Form
                {
                    FormID = 1,
                    Description = "FORM 3 -SDZEE - Financeiro Exportações",
                    SmallDescription = "Financeiro Exportações",
                    PeriodicityID=4,
                    FormTypeID=1
                },
                new Form
                {
                    FormID = 2,
                    Description = "FORM 3 -SDZEE - Financeiro",
                    SmallDescription = "Financeiro",
                    PeriodicityID=4,
                    FormTypeID=2
                },
                new Form
                {
                    FormID = 3,
                    Description = "FORM 1 -SDZEE - Operacional Licenças",
                    SmallDescription = "Operacional Licenças",
                    PeriodicityID=4,
                    FormTypeID=3
                },
                new Form
                {
                    FormID = 4,
                    Description = "FORM 2 -SDZEE - Operacional",
                    SmallDescription = "Operacional",
                    PeriodicityID=4,
                    FormTypeID=3
                },
                new Form
                {
                    FormID = 5,
                    Description = "FORM 3 -SDZEE - Recursos Humanos",
                    SmallDescription = "Recursos Humanos",
                    PeriodicityID=4,
                    FormTypeID=3
                },
                new Form
                {
                    FormID = 6,
                    Description = "FORM 3 -SDZEE - Recursos Humanos Qualificação",
                    SmallDescription = "Recursos Humanos Qualificação",
                    PeriodicityID=4,
                    FormTypeID=3
                }
            };

            return forms;
        }
    }

    public static class CxForm
    {
        public static IQueryable<Form> GetFormsByFunctionalAreaId(int? functionalAreaId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<Form> query = db.Forms;
            query = query.Where(p => (functionalAreaId == null | (p.FormType.FunctionalAreaID == functionalAreaId)));

            return query;
        }

        public static void UpdateRecord(int code, string description, string smallDescription, int periodicityId, int formTypeId, int sliderAlarmUser, int sliderAlarmManager, int sliderAlarmExecutive)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.Forms where c.FormID == code select c).First();
                if (entity != null)
                {
                    entity.Description = description;
                    entity.SmallDescription = smallDescription;
                    entity.PeriodicityID = periodicityId;
                    entity.FormTypeID = formTypeId;
                    entity.AlarmRangeUser = sliderAlarmUser;
                    entity.AlarmRangeManager = sliderAlarmManager;
                    entity.AlarmRangeExecutive = sliderAlarmExecutive;
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from Forms where FormId={0}", code);
                var myItem = (from c in db.Forms where c.FormID == code select c).First();
                if (myItem != null)
                {
                    db.Forms.Remove(myItem);
                    db.SaveChanges();
                }
            }
        }

        public static void CreateRecord(string description, string smallDescription, int periodicityId, int formTypeId, int sliderAlarmUser, int sliderAlarmManager, int sliderAlarmExecutive)
        {
            using (var db = new Models.SmizeeContext())
            {
                Form form = db.Forms.FirstOrDefault(cp => cp.Description == description);

                if (form != null)
                    throw new Exception("Duplicate Record!");

                Form entity = new Form();
                entity.Description = description;
                entity.SmallDescription = smallDescription;
                entity.PeriodicityID = periodicityId;
                entity.FormTypeID = formTypeId;
                entity.AlarmRangeUser = sliderAlarmUser;
                entity.AlarmRangeManager = sliderAlarmManager;
                entity.AlarmRangeExecutive = sliderAlarmExecutive;

                db.Forms.Add(entity);
                db.SaveChanges();
            }
        }

    }

}