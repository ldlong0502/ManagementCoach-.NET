namespace ManagementCoach.Migrations
{
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelteCascadeProvince : DbMigration
    {
        public override void Up()
        {
			Sql("ALTER TABLE dbo.\"RestAreas\"DROP CONSTRAINT \"FK_dbo.RestAreas_dbo.Provinces_ProvinceId\", ADD CONSTRAINT \"FK_dbo.RestAreas_dbo.Provinces_ProvinceId\" FOREIGN KEY (\"ProvinceId\") REFERENCES dbo.\"Provinces\" ON DELETE CASCADE;");
        }
        
        public override void Down()
        {
        }
    }
}
