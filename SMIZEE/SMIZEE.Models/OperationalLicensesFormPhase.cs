using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SMIZEE.Models
{
    public class OperationalLicensesFormPhase
    {
        public int OperationalLicensesFormPhaseID { get; set; }
        public int OperationalLicensesFormID { get; set; }
        public int? DevelopmentPhaseID { get; set; }
        public int? ProductionUnitsNumber { get; set; }

        public virtual OperationalLicensesForm OperationalLicensesForm { get; set; }
        public virtual DevelopmentPhase DevelopmentPhase { get; set; }
    }
    public static class CxOperationalLicensesFormPhase
    {
        public static void Delete(int operationalLicensesFormID)
        {
            // Query the database for the rows to be deleted.
            using (var db = new Models.SmizeeContext())
            {
                //db.Database.ExecuteSqlCommand("delete from FinancialExportForms where FinancialExportFormId={0}", code);
                IQueryable<OperationalLicensesFormPhase> myItem = from c in db.OperationalLicensesFormPhases where c.OperationalLicensesFormID == operationalLicensesFormID select c;
                foreach (OperationalLicensesFormPhase ec in myItem)
                {
                    db.OperationalLicensesFormPhases.Remove(ec);
                }
                db.SaveChanges();
            }

        }

        public static int? GetFormDevelopmentPhase(int operationalLicensesFormID, int developmentPhaseID)
        {
            int? productionUnitsNumber;

            var db = new Models.SmizeeContext();
            using (SmizeeContext Context = new SmizeeContext())
            {
                OperationalLicensesFormPhase formDevPhase = Context.OperationalLicensesFormPhases.FirstOrDefault(o => o.OperationalLicensesFormID == operationalLicensesFormID & o.DevelopmentPhaseID == developmentPhaseID);
                if (formDevPhase == null)
                    return null;
                productionUnitsNumber = formDevPhase.ProductionUnitsNumber;
            }
            return productionUnitsNumber;
        }

    }

}