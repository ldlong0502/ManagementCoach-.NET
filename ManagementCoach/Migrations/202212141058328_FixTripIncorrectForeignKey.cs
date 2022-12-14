namespace ManagementCoach.Migrations
{
	using ManagementCoach.BE.Entities;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTripIncorrectForeignKey : DbMigration
    {
        public override void Up()
        {
			Sql("ALTER TABLE dbo.\"Tickets\" DROP CONSTRAINT \"FK_dbo.Tickets_dbo.Trips_TripId\"");
			Sql("ALTER TABLE dbo.\"Trips\" DROP CONSTRAINT \"PK_dbo.Trips\"");
			Sql("ALTER TABLE dbo.\"Trips\" ADD PRIMARY KEY (\"Id\")");
			Sql("ALTER TABLE dbo.\"Trips\" ADD CONSTRAINT \"FK_dbo.Tickets_dbo.Trips_TripId\" FOREIGN KEY (\"Id\") REFERENCES dbo.\"Routes\" (\"Id\") MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION");
		}

		public override void Down()
        {
      
        }
    }
}
