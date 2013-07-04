using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class DevelopmentPhase
    {
        public int DevelopmentPhaseID { get; set; }

        [Required, StringLength(25), Display(Name = "Development Phase Small Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Development Phase Description")]
        public string Description { get; set; }
    }

    public static class DevelopmentPhaseStart
    {
        public static List<DevelopmentPhase> GetForms()
        {
            var forms = new List<DevelopmentPhase> {
                new DevelopmentPhase
                {
                    DevelopmentPhaseID = 1,
                    Description = "Iniciado",
                    SmallDescription = "Iniciado"
                },
                new DevelopmentPhase
                {
                    DevelopmentPhaseID = 2,
                    Description = "Concluído",
                    SmallDescription = "Concluído"
                },
                new DevelopmentPhase
                {
                    DevelopmentPhaseID = 3,
                    Description = "Terminado",
                    SmallDescription = "Terminado"
                }
            };

            return forms;
        }
    }

    public static class CxDevelopmentPhase
    {
        public static void UpdateRecord(int code, string SmallDescription, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.DevelopmentPhases where c.DevelopmentPhaseID == code select c).First();
                if (myItem != null)
                {
                    myItem.SmallDescription = SmallDescription;
                    myItem.Description = description;
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                db.Database.ExecuteSqlCommand("delete from DevelopmentPhases where DevelopmentPhaseId={0}", code);
                //var myItem = (from c in db.Companies where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Companies.Remove(myItem);
                //    db.SaveChanges();
                //}
            }
        }

        public static void CreateRecord(string SmallDescription, string description)
        {
            using (var db = new Models.SmizeeContext())
            {
                DevelopmentPhase developmentPhase = db.DevelopmentPhases.FirstOrDefault(cp => cp.Description == description | cp.SmallDescription == SmallDescription);

                if (developmentPhase != null)
                    throw new Exception("Duplicate Record!");

                DevelopmentPhase entity = new DevelopmentPhase();
                entity.SmallDescription = SmallDescription;
                entity.Description = description;

                db.DevelopmentPhases.Add(entity);
                db.SaveChanges();
            }
        }
    }

}