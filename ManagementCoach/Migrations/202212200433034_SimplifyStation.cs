namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplifyStation : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Stations", new[] { "District" });
            DropColumn("dbo.Stations", "District");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stations", "District", c => c.String(nullable: false));
            CreateIndex("dbo.Stations", "District", unique: true);
        }
    }
}
