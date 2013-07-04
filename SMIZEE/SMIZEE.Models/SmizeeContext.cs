using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace SMIZEE.Models
{
    public class SmizeeContext : DbContext
    {
        public SmizeeContext()
            : base("DataContext")
        {
        }

        public DbSet<Audit> Audits { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LabourProfile> LabourProfiles { get; set; }
        public DbSet<Area> Areas{ get; set; }
        public DbSet<FunctionalArea> FunctionalAreas { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<DevelopmentPhase> DevelopmentPhases { get; set; }
        public DbSet<Periodicity> Periodicities { get; set; }
        public DbSet<InvestmentType> InvestmentTypes { get; set; }
        public DbSet<ProductionUnit> ProductionUnits { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Timeline> Timelines { get; set; }

        public DbSet<FormType> FormTypes { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FinancialExportForm> FinancialExportForms { get; set; }
        public DbSet<FinancialExportFormCountry> FinancialExportFormCountries { get; set; }
        public DbSet<FinancialForm> FinancialForms { get; set; }
        public DbSet<FinancialSDZEEForm> FinancialSDZEEForms { get; set; }

        public DbSet<OperationalLicensesForm> OperationalLicensesForms { get; set; }
        public DbSet<OperationalLicensesFormPhase> OperationalLicensesFormPhases { get; set; }
        public DbSet<OperationalForm> OperationalForms { get; set; }
        public DbSet<HumanResourceForm> HumanResourceForms { get; set; }
        public DbSet<HumanResourceQualificationForm> HumanResourceQualificationForms { get; set; }
        public DbSet<HumanResourceSDZEEForm> HumanResourceSDZEEForms { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProductionUnit> UserProductionUnits { get; set; }


    }
}
