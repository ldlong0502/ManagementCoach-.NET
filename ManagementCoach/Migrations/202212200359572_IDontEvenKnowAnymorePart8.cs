namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IDontEvenKnowAnymorePart8 : DbMigration
    {
        public override void Up()
        {
			RenameColumn("dbo.Tickets", "Passenger_Id", "PassengerId");
        }
        
        public override void Down()
        {
        }
    }
}
