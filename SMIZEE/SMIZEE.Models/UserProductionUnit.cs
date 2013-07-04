using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class UserProductionUnit
    {
        public int UserProductionUnitID { get; set; }

        public Guid UserID { get; set; }
        public int ProductionUnitID { get; set; }

        public virtual User User { get; set; }
        public virtual ProductionUnit ProductionUnit { get; set; }

    }

    public static class CxUserProductionUnit
    {
        public static void Insert(Guid userId, int productionUnitID)
        {
            using (var db = new Models.SmizeeContext())
            {
                UserProductionUnit entity = new UserProductionUnit();
                entity.UserID = userId;
                entity.ProductionUnitID = productionUnitID;

                db.UserProductionUnits.Add(entity);
                db.SaveChanges();
            }
        }

        public static void Delete(Guid userId)
        {
            //Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from UserProductionUnits where UserId={0}", userId);
                IQueryable<UserProductionUnit> myItem = from c in db.UserProductionUnits where c.UserID == userId select c;
                foreach (UserProductionUnit ec in myItem)
                {
                    db.UserProductionUnits.Remove(ec);
                }
                db.SaveChanges();
            }
        }

        public static IQueryable<UserProductionUnit> GetListByUserId(Guid? userId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<UserProductionUnit> query = db.UserProductionUnits;
            query = query.Where(p => (userId == null | (p.UserID == userId)));

            return query;
        }

        public static IQueryable<UserProductionUnit> GetProductionUnitsByUserId(SmizeeContext db, Guid? userId)
        {
            IQueryable<UserProductionUnit> query = db.UserProductionUnits;
            query = query.Where(p => (userId == null | (p.UserID == userId)));

            return query;
        }

        public static IQueryable<UserProductionUnit> GetListByProductionUnit(int productionUnitId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<UserProductionUnit> query = db.UserProductionUnits;
            query = query.Where(p => (productionUnitId == null | (p.ProductionUnitID == productionUnitId)));

            return query;
        }

        public static Boolean IsUserProductionUnit(Guid? userId, int productionUnitId)
        {
            int count = 0;
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.UserProductionUnits where c.UserID == userId & c.ProductionUnitID==productionUnitId select c);
                count = myItem.ToList().Count();
            }
            return count>0;
        }
    }

}