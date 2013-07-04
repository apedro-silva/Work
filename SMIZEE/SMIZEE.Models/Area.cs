using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class Area
    {
        public int AreaID { get; set; }

        [Required, StringLength(25), Display(Name = "Area Small Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Area Description")]
        public string Description { get; set; }

        public int? Hectares { get; set; }
    }

    public static class AreaStart
    {
        public static List<Area> GetAreas()
        {
            var areas = new List<Area> {
                new Area
                {
                    AreaID = 1,
                    Description = "Area1",
                    SmallDescription = "Area1",
                    Hectares = 1000
                },
                new Area
                {
                    AreaID = 2,
                    Description = "Area2",
                    SmallDescription = "Area2",
                    Hectares = 2000
                },
                new Area
                {
                    AreaID = 3,
                    Description = "Area3",
                    SmallDescription = "Area3",
                    Hectares = 3000
                }
            };

            return areas;
        }
    }
    public static class CxArea
    {
        public static void UpdateRecord(int code, string SmallDescription, string description, int? hectares)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.Areas where c.AreaID == code select c).First();
                if (entity != null)
                {
                    entity.SmallDescription = SmallDescription;
                    entity.Description = description;
                    entity.Hectares = hectares;
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                db.Database.ExecuteSqlCommand("delete from Areas where AreaId={0}", code);
                //var myItem = (from c in db.Companies where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Companies.Remove(myItem);
                //    db.SaveChanges();
                //}
            }
        }

        public static void CreateRecord(string SmallDescription, string description, int? hectares)
        {
            using (var db = new Models.SmizeeContext())
            {
                Area area = db.Areas.FirstOrDefault(cp => cp.Description == description | cp.SmallDescription == SmallDescription);

                if (area != null)
                    throw new Exception("Duplicate Record!");

                Area entity = new Area();
                entity.SmallDescription = SmallDescription;
                entity.Description = description;
                entity.Hectares = hectares;
                db.Areas.Add(entity);
                db.SaveChanges();
            }
        }
    }

}