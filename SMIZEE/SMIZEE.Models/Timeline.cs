using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class Timeline
    {
        public int TimelineID { get; set; }

        [Required, StringLength(25), Display(Name = "Timeline Samll Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Timeline Description")]
        public string Description { get; set; }
    }

    public static class TimelineStart
    {
        public static List<Timeline> GetForms()
        {
            var forms = new List<Timeline> {
                new Timeline
                {
                    TimelineID = 1,
                    Description = "1º Trimestre",
                    SmallDescription = "1T"
                },
                new Timeline
                {
                    TimelineID = 2,
                    Description = "2º Trimeste",
                    SmallDescription = "2T"
                },
                new Timeline
                {
                    TimelineID = 3,
                    Description = "3º Trimestre",
                    SmallDescription = "3T"
                },
                new Timeline
                {
                    TimelineID = 4,
                    Description = "4º Trimestre",
                    SmallDescription = "4T"
                },
                new Timeline
                {
                    TimelineID = 5,
                    Description = "1º Semestre",
                    SmallDescription = "1S"
                },
                new Timeline
                {
                    TimelineID = 6,
                    Description = "2º Semestre",
                    SmallDescription = "2S"
                }
            };

            return forms;
        }
    }

    public static class CxTimeline
    {
        public static void UpdateRecord(int code, string SmallDescription, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.Timelines where c.TimelineID == code select c).First();
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
                //db.Database.ExecuteSqlCommand("delete from Timelines where TimelineId={0}", code);
                var myItem = (from c in db.Timelines where c.TimelineID == code select c).First();
                if (myItem != null)
                {
                    db.Timelines.Remove(myItem);
                    db.SaveChanges();
                }
            }
        }

        public static void CreateRecord(string smallDescription, string description)
        {
            using (var db = new Models.SmizeeContext())
            {
                Timeline timeline  = db.Timelines.FirstOrDefault(cp => cp.Description == description | cp.SmallDescription == smallDescription);

                if (timeline != null)
                    throw new Exception("Duplicate Record!");

                Timeline entity = new Timeline();
                entity.SmallDescription = smallDescription;
                entity.Description = description;

                db.Timelines.Add(entity);
                db.SaveChanges();
            }
        }
    }
}