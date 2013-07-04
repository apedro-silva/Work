using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class ProductionUnit
    {
        public int ProductionUnitID { get; set; }

        [Required, StringLength(200), Display(Name = "Description")]
        public string Description { get; set; }

        public int AreaID { get; set; }
        public virtual Area Area { get; set; }

        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        
        public int SectorID { get; set; }
        public virtual Sector Sector { get; set; }

        public int? Hectares { get; set; }
    }

    public static class ProductionUnitStart
    {
        public static List<ProductionUnit> GetProductionUnits()
        {
            var productionUnits = new List<ProductionUnit> {
                new ProductionUnit
                {
                    ProductionUnitID = 1,
                    Description="Sonangol-Financeiros",
                    AreaID = 1,
                    CompanyID=1,
                    SectorID=1
                },
                new ProductionUnit
                {
                    ProductionUnitID = 2,
                    Description="BESA-Recursos Humanos",
                    AreaID = 1,
                    CompanyID=2,
                    SectorID=2
                },
                new ProductionUnit
                {
                    ProductionUnitID = 3,
                    Description="Standard Bank-Marketing",
                    AreaID = 1,
                    CompanyID=3,
                    SectorID=3
                }
            };

            return productionUnits;
        }
    }

    public static class CxProductionUnit
    {
        public static void UpdateRecord(int code, string description, int AreaId, int CompanyId, int SectorId, int? Hectares)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.ProductionUnits where c.ProductionUnitID == code select c).First();
                if (entity != null)
                {
                    entity.Description = description;
                    entity.AreaID = AreaId;
                    entity.CompanyID = CompanyId;
                    entity.SectorID = SectorId;
                    entity.Hectares = Hectares;
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                db.Database.ExecuteSqlCommand("delete from ProductionUnits where ProductionUnitId={0}", code);
                //var myItem = (from c in db.Companies where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Companies.Remove(myItem);
                //    db.SaveChanges();
                //}
            }
        }

        public static void CreateRecord(string description, int AreaId, int CompanyId, int SectorId, int? Hectares)
        {
            using (var db = new Models.SmizeeContext())
            {
                ProductionUnit productionUnit = db.ProductionUnits.FirstOrDefault(cp => cp.Description == description);

                if (productionUnit != null)
                    throw new Exception("Duplicate Record!");

                ProductionUnit entity = new ProductionUnit();
                entity.Description = description;
                entity.AreaID = AreaId;
                entity.CompanyID = CompanyId;
                entity.SectorID = SectorId;
                entity.Hectares = Hectares;

                db.ProductionUnits.Add(entity);
                db.SaveChanges();
            }
        }

        public static ProductionUnit GetProductionUnit(int? ProductionUnitId)
        {
            ProductionUnit ProductionUnit = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                ProductionUnit = Context.ProductionUnits.FirstOrDefault(cp => cp.ProductionUnitID == ProductionUnitId);
            }
            return ProductionUnit;

        }

        public static List<ProductionUnit> GetProductionUnitsList()
        {
            var db = new Models.SmizeeContext();
            IQueryable<ProductionUnit> query = db.ProductionUnits;
            return query.ToList();
        }
        public static List<ProductionUnit> GetListByCompanyId(int companyId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<ProductionUnit> query = db.ProductionUnits;
            query = query.Where(p => (companyId == 0 | (p.CompanyID == companyId)));
            return query.ToList();

        }

    }
}