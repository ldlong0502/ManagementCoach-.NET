namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestArea : DbMigration
    {
        public override void Up()
        {
			Sql("CREATE SEQUENCE dbo.\"RestAreas_Id_seq\" INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 OWNED BY dbo.\"RestAreas\".\"Id\"");
			Sql("ALTER TABLE dbo.\"RestAreas\" ALTER COLUMN \"Id\" SET DEFAULT nextval('dbo.\"RestAreas_Id_seq\"'::regclass)");
		}

		public override void Down()
        {
        }
    }
}
