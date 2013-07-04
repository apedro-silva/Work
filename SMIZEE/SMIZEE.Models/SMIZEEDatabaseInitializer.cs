using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;

namespace SMIZEE.Models
{
    public class SMIZEEDatabaseInitializer : DropCreateDatabaseIfModelChanges<SmizeeContext>
    {
        protected override void Seed(SmizeeContext context)
        {
            CompanyStart.GetCompanies().ForEach(c => context.Companies.Add(c)); 
            CountryStart.GetCountries().ForEach(c => context.Countries.Add(c)); 
            AreaStart.GetAreas().ForEach(c => context.Areas.Add(c)); 
            SectorStart.GetSectors().ForEach(c => context.Sectors.Add(c)); 
            PeriodicityStart.GetPeriodicities().ForEach(c => context.Periodicities.Add(c)); 
            ProductionUnitStart.GetProductionUnits().ForEach(c => context.ProductionUnits.Add(c)); 
            DevelopmentPhaseStart.GetForms().ForEach(c => context.DevelopmentPhases.Add(c));
            StateStart.GetForms().ForEach(c => context.States.Add(c));
            FunctionalAreaStart.GetFunctionalAreas().ForEach(c => context.FunctionalAreas.Add(c));
            FormTypeStart.GetFormTypes().ForEach(c => context.FormTypes.Add(c));
            FormStart.GetForms().ForEach(c => context.Forms.Add(c));
            
            context.SaveChanges();

            CxUser.Register("Administrator", "passwd", "administrator@softfinanca.com", true, "Administrator", "Administrator", null);
            System.Web.Security.Roles.CreateRole("Admin");
            System.Web.Security.Roles.CreateRole("User");
            System.Web.Security.Roles.AddUserToRole("Administrator", "Admin");

        }
    }
}