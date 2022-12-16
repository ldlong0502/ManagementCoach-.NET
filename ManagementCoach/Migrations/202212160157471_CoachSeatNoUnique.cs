namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoachSeatNoUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CoachSeats", new[] { "Name" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.CoachSeats", "Name", unique: true);
        }
    }
}
