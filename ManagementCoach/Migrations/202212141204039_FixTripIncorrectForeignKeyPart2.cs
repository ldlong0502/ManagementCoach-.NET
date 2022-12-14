namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTripIncorrectForeignKeyPart2 : DbMigration
    {
        public override void Up()
        {
			Sql("ALTER TABLE dbo.\"Tickets\" ADD CONSTRAINT \"FK_dbo.Tickets_dbo.Trips_TripId\" FOREIGN KEY (\"TripId\") REFERENCES dbo.\"Trips\" (\"Id\") MATCH SIMPLE ON UPDATE NO ACTION ON DELETE CASCADE");
			Sql("ALTER TABLE dbo.\"Trips\" DROP CONSTRAINT \"FK_dbo.Tickets_dbo.Trips_TripId\"");
			
			Sql("ALTER TABLE dbo.\"Trips\" DROP CONSTRAINT \"FK_dbo.Trips_dbo.Routes_Id\"");
			Sql("ALTER TABLE dbo.\"Trips\" ADD CONSTRAINT \"FK_dbo.Trips_dbo.Routes_Id\" FOREIGN KEY (\"RouteId\") REFERENCES dbo.\"Routes\" (\"Id\") MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION");

		}

		public override void Down()
        {
        }
    }
}
