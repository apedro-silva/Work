using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SMIZEE.Models
{

    public class Country
    {
        [Key]
        public int CountryID { get; set; }

        [Required, StringLength(25), Display(Name = "Country Small Description")]
        public string SmallDescription { get; set; }

        [Required, StringLength(100), Display(Name = "Country Description")]
        public string Description { get; set; }

        [Required, StringLength(3), Display(Name = "ISO Code")]
        public string ISOCode { get; set; }
    }

    public static class CountryStart
    {
        public static List<Country> GetCountries()
        {
            var countries = new List<Country> {
                new Country
                {
                    CountryID = 1,
                    Description = "Portugal",
                    ISOCode = "351",
                    SmallDescription ="PT"
                },
                new Country
                {
                    CountryID = 1,
                    Description = "Espanha",
                    ISOCode = "356",
                    SmallDescription ="ES"
                },
                new Country
                {
                    CountryID = 1,
                    Description = "Estados Unidos da Améria",
                    ISOCode = "355",
                    SmallDescription ="US"
                },
                new Country
                {
                    CountryID = 1,
                    Description = "Moçambique",
                    ISOCode = "354",
                    SmallDescription ="MO"
                },
                new Country
                {
                    CountryID = 1,
                    Description = "Alemanha",
                    ISOCode = "357",
                    SmallDescription ="AM"
                },
                new Country
                {
                    CountryID = 1,
                    Description = "Polónia",
                    ISOCode = "359",
                    SmallDescription ="PL"
                },
                new Country
                {
                    CountryID = 1,
                    Description = "França",
                    ISOCode = "353",
                    SmallDescription ="FR"
                }
            };

            return countries;
        }
    }

    public static class CxCountry
    {
        public static void CreateRecord(string smallDescription, string description, string isoCode)
        {
            using (var db = new Models.SmizeeContext())
            {
                Country country = db.Countries.FirstOrDefault(cp => cp.Description == description | cp.SmallDescription == smallDescription | cp.ISOCode == isoCode);

                if (country != null)
                    throw new Exception("Duplicate Record!");

                Country entity = new Country();
                entity.Description = description;
                entity.SmallDescription = smallDescription;
                entity.ISOCode = isoCode;

                db.Countries.Add(entity);
                db.SaveChanges();
            }
        }

        public static void UpdateRecord(int countryId, string smallDescription, string description, string isoCode)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                var myItem = (from c in db.Countries where c.CountryID == countryId select c).First();
                if (myItem != null)
                {
                    myItem.Description = description;
                    myItem.SmallDescription = smallDescription;
                    myItem.ISOCode = isoCode;
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecord(int code)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                db.Database.ExecuteSqlCommand("delete from Countries where CountryId={0}", code);
                //var myItem = (from c in db.Companies where c.CompanyID == code select c).First();
                //if (myItem != null)
                //{
                //    db.Companies.Remove(myItem);
                //    db.SaveChanges();
                //}
            }

        }

        public static Country GetCountry(int? CountryId)
        {
            Country Country = null;
            using (SmizeeContext Context = new SmizeeContext())
            {
                Country = Context.Countries.FirstOrDefault(cp => cp.CountryID == CountryId);
            }
            return Country;

        }

        public static List<Country> GetCountriesList()
        {
            var db = new Models.SmizeeContext();
            IQueryable<Country> query = db.Countries;
            return query.ToList();
        }

    }

}