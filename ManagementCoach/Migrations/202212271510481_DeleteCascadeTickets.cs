namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteCascadeTickets : DbMigration
    {
        public override void Up()
        {
			Sql("ALTER TABLE dbo.\"Tickets\"DROP CONSTRAINT \"FK_dbo.Tickets_dbo.CoachSeats_CoachSeatId\", ADD CONSTRAINT \"FK_dbo.Tickets_dbo.CoachSeats_CoachSeatId\" FOREIGN KEY (\"CoachSeatId\") REFERENCES dbo.\"CoachSeats\" ON DELETE CASCADE;");
			Sql("ALTER TABLE dbo.\"Tickets\"DROP CONSTRAINT \"FK_dbo.Tickets_dbo.Passengers_Passenger_Id\", ADD CONSTRAINT \"FK_dbo.Tickets_dbo.Passengers_Passenger_Id\" FOREIGN KEY (\"PassengerId\") REFERENCES dbo.\"Passengers\" ON DELETE CASCADE;");
		}

		public override void Down()
        {
        }
    }
}
