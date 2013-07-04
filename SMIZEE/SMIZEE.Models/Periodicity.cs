using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class Periodicity
    {
        [ScaffoldColumn(false)]
        public int PeriodicityID { get; set; }

        [Required, StringLength(100), Display(Name = "Periodicity Description")]
        public string Description { get; set; }

        [Required, StringLength(2), Display(Name = "Periodicity Code")]
        public string Code { get; set; }

    }

    public static class PeriodicityStart
    {
        public static List<Periodicity> GetPeriodicities()
        {
            var periodicities = new List<Periodicity> {
                new Periodicity
                {
                    PeriodicityID = 1,
                    Description = "Diário",
                    Code = "D"
                },
                new Periodicity
                {
                    PeriodicityID = 2,
                    Description = "Semanal",
                    Code = "W"
                },
                new Periodicity
                {
                    PeriodicityID = 3,
                    Description = "Mensal",
                    Code = "M"
                },
                new Periodicity
                {
                    PeriodicityID = 4,
                    Description = "Trimestral",
                    Code = "Q"
                },
                new Periodicity
                {
                    PeriodicityID = 5,
                    Description = "Semestral",
                    Code = "S"
                },
                new Periodicity
                {
                    PeriodicityID = 6,
                    Description = "Anual",
                    Code = "Y"
                }
            };

            return periodicities;
        }
    }

    public static class CxPeriodicity
    {
        public static void UpdateRecord(int periodicityId, string description, string code)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.Periodicities where c.PeriodicityID == periodicityId select c).First();
                if (myItem != null)
                {
                    myItem.Description = description;
                    myItem.Code = code;
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecord(int periodicityId)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                db.Database.ExecuteSqlCommand("delete from Periodicities where PeriodicityId={0}", periodicityId);
                //var myItem = (from c in db.Companies where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Companies.Remove(myItem);
                //    db.SaveChanges();
                //}
            }
        }

        public static void CreateRecord(string description, string code)
        {
            using (var db = new Models.SmizeeContext())
            {
                Periodicity periodicity = db.Periodicities.FirstOrDefault(cp => cp.Description == description | cp.Code == code);

                if (periodicity != null)
                    throw new Exception("Duplicate Record!");

                Periodicity entity = new Periodicity();
                entity.Description = description;
                entity.Code = code;

                db.Periodicities.Add(entity);
                db.SaveChanges();
            }
        }
    }
}