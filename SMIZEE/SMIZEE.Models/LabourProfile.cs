using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class LabourProfile  
    {
        [ScaffoldColumn(false)]
        public int LabourProfileID { get; set; }

        [Required, StringLength(100), Display(Name = "SmallDescription")]
        public string SmallDescription { get; set; }

        [Required, StringLength(10000), Display(Name = "LabourProfile Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }

    public static class CxLabourProfile
    {
        public static void UpdateRecord(int code, string SmallDescription, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.LabourProfiles where c.LabourProfileID == code select c).First();
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
                db.Database.ExecuteSqlCommand("delete from LabourProfiles where LabourProfileId={0}", code);
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
                LabourProfile labourProfile = db.LabourProfiles.FirstOrDefault(cp => cp.Description == description | cp.SmallDescription == SmallDescription);

                if (labourProfile != null)
                    throw new Exception("Duplicate Record!");

                LabourProfile entity = new LabourProfile();
                entity.SmallDescription = SmallDescription;
                entity.Description = description;

                db.LabourProfiles.Add(entity);
                db.SaveChanges();
            }
        }

    }
}