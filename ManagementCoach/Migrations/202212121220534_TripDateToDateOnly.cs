namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TripDateToDateOnly : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "Date", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trips", "Date");
        }
    }
}
