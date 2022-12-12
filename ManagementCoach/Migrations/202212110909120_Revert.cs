namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Revert : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "Station_Id", c => c.Int(nullable: true));
		}

		public override void Down()
        {
            DropColumn("dbo.Routes", "Station_Id");
		}
	}
}
