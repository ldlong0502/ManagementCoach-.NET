namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRouteSchema : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Routes", "Station_Id");
		}

		public override void Down()
        {
            AddColumn("dbo.Routes", "Station_Id", c => c.Int(nullable: true));
		}
	}
}
