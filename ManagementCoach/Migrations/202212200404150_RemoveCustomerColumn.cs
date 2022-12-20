namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCustomerColumn : DbMigration
    {
        public override void Up()
        {
			DropColumn("dbo.Tickets", "CustomerId");
		}
        
        public override void Down()
        {
        }
    }
}
