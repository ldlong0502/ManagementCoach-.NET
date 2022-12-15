namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestArea2 : DbMigration
    {
        public override void Up()
        {
			DropForeignKey("dbo.RestAreas", "FK_dbo.RestAreas_dbo.Provinces_Id");
			AddForeignKey("dbo.RestAreas", "ProvinceId", "dbo.Provinces", "Id");
		}

		public override void Down()
        {
        }
    }
}
