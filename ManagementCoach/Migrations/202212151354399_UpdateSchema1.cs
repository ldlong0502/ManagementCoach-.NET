namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchema1 : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.Users", "ImageUrl", c => c.String());
			AddColumn("dbo.Coaches", "ImageUrl", c => c.String());
			DropColumn("dbo.CoachSeats", "Status");

			AddColumn("dbo.Tickets", "CoachSeatId", c => c.Int(nullable: false, identity: true));
			AddForeignKey("dbo.Tickets", "CoachSeatId", "dbo.CoachSeats", "Id");
		}
        
        public override void Down()
        {
        }
    }
}
