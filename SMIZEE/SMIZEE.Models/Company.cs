using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class Company
    {
        public int CompanyID { get; set; }

        [Required, StringLength(100), Display(Name = "Name")]
        public string CompanyName { get; set; }

        [StringLength(100), Display(Name = "Company Description")]
        public string Description { get; set; }

        [StringLength(200), Display(Name = "Company URL")]
        public string CompanyURL { get; set; }
    }

    public static class CompanyStart
    {
        public static List<Company> GetCompanies()
        {
            var companies = new List<Company> {
                new Company
                {
                    CompanyID = 1,
                    CompanyName = "Sonangol",
                    Description = "Sonangol"
                },
                new Company
                {
                    CompanyID = 2,
                    CompanyName = "BESA",
                    Description = "BESA"
                },
                new Company
                {
                    CompanyID = 3,
                    CompanyName = "Standard Bank",
                    Description = "Standard Bank"
                }
            };

            return companies;
        }
    }

    public static class CxCompany
    {
        public static Company GetCompany(int? companyId)
        {
            Company company = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                company = Context.Companies.FirstOrDefault(cp => cp.CompanyID == companyId);
            }
            return company;

        }

        public static List<Company> GetCompaniesList()
        {
            var db = new Models.SmizeeContext();
            IQueryable<Company> query = db.Companies;
            return query.ToList();
        }

        public static void UpdateRecord(int code, string companyName, string description)
        {

            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var entity = (from c in db.Companies where c.CompanyID == code select c).First();
                if (entity != null)
                {
                    entity.CompanyName = companyName;
                    entity.Description = description;
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                db.Database.ExecuteSqlCommand("delete from Companies where CompanyId={0}", code);
                //var myItem = (from c in db.Company where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Company.Remove(myItem);
                //    db.SaveChanges();
                //}
            }

        }

        public static void CreateRecord(string companyName, string description)
        {
            using (var db = new Models.SmizeeContext())
            {
                Company country = db.Companies.FirstOrDefault(cp => cp.Description == description | cp.CompanyName == companyName);

                if (country != null)
                    throw new Exception("Duplicate Record!");

                Company entity = new Company();
                entity.CompanyName = companyName;
                entity.Description = description;

                db.Companies.Add(entity);
                db.SaveChanges();
            }
        }

    }

}