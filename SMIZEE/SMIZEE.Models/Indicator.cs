using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class Indicator
    {
        public int IndicatorID { get; set; }

        [Required, StringLength(25), Display(Name = "Indicator Small Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Indicator Description")]
        public string Description { get; set; }
    }

    public static class CxIndicator
    {
        public static void UpdateRecord(int code, string SmallDescription, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.Indicators where c.IndicatorID == code select c).First();
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
                db.Database.ExecuteSqlCommand("delete from Indicators where IndicatorId={0}", code);
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
                Indicator indicator = db.Indicators.FirstOrDefault(cp => cp.Description == description | cp.SmallDescription == SmallDescription);

                if (indicator != null)
                    throw new Exception("Duplicate Record!");

                Indicator entity = new Indicator();
                entity.SmallDescription = SmallDescription;
                entity.Description = description;

                db.Indicators.Add(entity);
                db.SaveChanges();
            }
        }

    }
}