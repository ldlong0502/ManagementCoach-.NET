namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RouteRestAreas", "RestArea_Id", "dbo.RestAreas");
            DropIndex("dbo.RouteRestAreas", new[] { "RestArea_Id" });
            DropColumn("dbo.RouteRestAreas", "RestAreaId");
            RenameColumn(table: "dbo.RouteRestAreas", name: "RestArea_Id", newName: "RestAreaId");
            AddColumn("dbo.Stations", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.RouteRestAreas", "RestAreaId", c => c.Int(nullable: false));
            AlterColumn("dbo.RouteRestAreas", "RestAreaId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.RouteRestAreas", new[] { "RouteId", "RestAreaId" });
            CreateIndex("dbo.RouteRestAreas", "RestAreaId");
            AddForeignKey("dbo.RouteRestAreas", "RestAreaId", "dbo.RestAreas", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteRestAreas", "RestAreaId", "dbo.RestAreas");
            DropIndex("dbo.RouteRestAreas", new[] { "RestAreaId" });
            DropPrimaryKey("dbo.RouteRestAreas");
            AlterColumn("dbo.RouteRestAreas", "RestAreaId", c => c.Int());
            AlterColumn("dbo.RouteRestAreas", "RestAreaId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Stations", "Name");
            RenameColumn(table: "dbo.RouteRestAreas", name: "RestAreaId", newName: "RestArea_Id");
            AddColumn("dbo.RouteRestAreas", "RestAreaId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.RouteRestAreas", "RestArea_Id");
            AddForeignKey("dbo.RouteRestAreas", "RestArea_Id", "dbo.RestAreas", "Id");
        }
    }
}
