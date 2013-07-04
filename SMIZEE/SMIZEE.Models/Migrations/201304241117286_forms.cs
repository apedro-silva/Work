namespace SMIZEE.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forms : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OperationalLicensesForms", "ProductionUnitID", "dbo.ProductionUnits");
            DropIndex("dbo.OperationalLicensesForms", new[] { "ProductionUnitID" });
            DropColumn("dbo.OperationalLicensesForms", "ProductionUnitID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OperationalLicensesForms", "ProductionUnitID", c => c.Int(nullable: false));
            CreateIndex("dbo.OperationalLicensesForms", "ProductionUnitID");
            AddForeignKey("dbo.OperationalLicensesForms", "ProductionUnitID", "dbo.ProductionUnits", "ProductionUnitID", cascadeDelete: true);
        }
    }
}
