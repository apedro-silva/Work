using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class FinancialExportFormCountry
    {
        public int FinancialExportFormCountryID { get; set; }
        public int FinancialExportFormID { get; set; }
        public decimal? ExportAmount { get; set; }
        public int CountryID { get; set; }

        public virtual FinancialExportForm FinancialExportForm { get; set; }
        public virtual Country Country { get; set; }

    }
    public static class CxFinancialExportFormCountry
    {
        public static void Delete(int financialExportFormID)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from FinancialExportForms where FinancialExportFormId={0}", code);
                IQueryable<FinancialExportFormCountry> myItem = from c in db.FinancialExportFormCountries where c.FinancialExportFormID == financialExportFormID select c;
                foreach (FinancialExportFormCountry ec in myItem)
                {
                    db.FinancialExportFormCountries.Remove(ec);
                }
                db.SaveChanges();
            }

        }

        public static IQueryable<FinancialExportFormCountry> GetForms(int financialExportFormId)
        {
            var db = new Models.SmizeeContext();
            IQueryable<FinancialExportFormCountry> forms = from f in db.FinancialExportFormCountries
                                                                where f.FinancialExportFormID == financialExportFormId select f;

            return forms;
        }
    }

}