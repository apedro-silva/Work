using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class State
    {
        public int StateID { get; set; }

        [Required, StringLength(25), Display(Name = "State Samll Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "State Description")]
        public string Description { get; set; }
    }

    public static class StateStart
    {
        public static List<State> GetForms()
        {
            var forms = new List<State> {
                new State
                {
                    StateID = 1,
                    Description = "Inicializado",
                    SmallDescription = "Inicializado"
                },
                new State
                {
                    StateID = 2,
                    Description = "Gravado",
                    SmallDescription = "Gravado"
                },
                new State
                {
                    StateID = 3,
                    Description = "Submetido",
                    SmallDescription = "Submetido"
                },
                new State
                {
                    StateID = 4,
                    Description = "Aprovado",
                    SmallDescription = "Aprovado"
                }
            };

            return forms;
        }
    }
    public static class CxStates
    {
        public static List<State> GetList()
        {
            var db = new Models.SmizeeContext();
            IQueryable<State> query = db.States;
            return query.ToList();

        }
        public static void UpdateRecord(int code, string SmallDescription, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.States where c.StateID == code select c).First();
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
                db.Database.ExecuteSqlCommand("delete from States where StateId={0}", code);
                //var myItem = (from c in db.Companies where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Companies.Remove(myItem);
                //    db.SaveChanges();
                //}
            }
        }

        public static void CreateRecord(string smallDescription, string description)
        {
            using (var db = new Models.SmizeeContext())
            {
                State state = db.States.FirstOrDefault(cp => cp.Description == description | cp.SmallDescription == smallDescription);

                if (state != null)
                    throw new Exception("Duplicate Record!");


                State entity = new State();
                entity.SmallDescription = smallDescription;
                entity.Description = description;

                db.States.Add(entity);
                db.SaveChanges();
            }
        }

    }

}