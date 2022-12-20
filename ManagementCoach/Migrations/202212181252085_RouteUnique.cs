namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RouteUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Routes", new[] { "OriginStationId" });
            DropIndex("dbo.Routes", new[] { "DestinationStationId" });
            CreateIndex("dbo.Routes", new[] { "OriginStationId", "DestinationStationId" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Routes", new[] { "OriginStationId", "DestinationStationId" });
            CreateIndex("dbo.Routes", "DestinationStationId");
            CreateIndex("dbo.Routes", "OriginStationId");
        }
    }
}
