namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDepartime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "DepartTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routes", "DepartTime");
        }
    }
}
