namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveRouteTimeRelatedColumnsToTrip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "DepartTime", c => c.Int(nullable: false));
            AddColumn("dbo.Trips", "EstimatedTime", c => c.Int(nullable: false));
            DropColumn("dbo.Trips", "Date");
            DropColumn("dbo.Routes", "DepartTime");
            DropColumn("dbo.Routes", "EstimatedTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Routes", "EstimatedTime", c => c.Int(nullable: false));
            AddColumn("dbo.Routes", "DepartTime", c => c.Int(nullable: false));
            AddColumn("dbo.Trips", "Date", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.Trips", "EstimatedTime");
            DropColumn("dbo.Trips", "DepartTime");
        }
    }
}
