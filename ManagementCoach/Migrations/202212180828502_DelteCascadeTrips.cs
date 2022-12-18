namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelteCascadeTrips : DbMigration
    {
        public override void Up()
        {
			Sql("ALTER TABLE dbo.\"Trips\"DROP CONSTRAINT \"FK_dbo.Trips_dbo.Routes_Id\", ADD CONSTRAINT \"FK_dbo.Trips_dbo.Routes_Id\" FOREIGN KEY (\"RouteId\") REFERENCES dbo.\"Routes\" ON DELETE CASCADE;");
		}

		public override void Down()
        {
        }
    }
}
