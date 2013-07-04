using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class Sector
    {
        public int SectorID { get; set; }

        [Required, StringLength(25), Display(Name = "Sector Small Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Sector Description")]
        public string Description { get; set; }
    }

    public static class SectorStart
    {
        public static List<Sector> GetSectors()
        {
            var sectors = new List<Sector> {
                new Sector
                {
                    SectorID = 1,
                    Description = "Sector1",
                    SmallDescription = "Sector1"
                },
                new Sector
                {
                    SectorID = 2,
                    Description = "Sector2",
                    SmallDescription = "Sector2"
                },
                new Sector
                {
                    SectorID = 3,
                    Description = "Sector3",
                    SmallDescription = "Sector3"
                }
            };

            return sectors;
        }
    }

    public static class CxSector
    {
        public static void UpdateRecord(int code, string SmallDescription, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.Sectors where c.SectorID == code select c).First();
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
                db.Database.ExecuteSqlCommand("delete from Sectors where SectorId={0}", code);
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
                Sector sector = db.Sectors.FirstOrDefault(cp => cp.Description == description | cp.SmallDescription == SmallDescription);

                if (sector != null)
                    throw new Exception("Duplicate Record!");

                Sector entity = new Sector();
                entity.SmallDescription = SmallDescription;
                entity.Description = description;

                db.Sectors.Add(entity);
                db.SaveChanges();
            }
        }

    }

}