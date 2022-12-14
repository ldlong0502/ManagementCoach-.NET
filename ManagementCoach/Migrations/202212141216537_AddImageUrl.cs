namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "ImageUrl", c => c.String());
            AddColumn("dbo.Routes", "ImageUrl", c => c.String());
            AddColumn("dbo.Passengers", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Passengers", "ImageUrl");
            DropColumn("dbo.Routes", "ImageUrl");
            DropColumn("dbo.Drivers", "ImageUrl");
        }
    }
}
