namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoIncrement : DbMigration
    {
        public override void Up()
        {
			Sql("CREATE SEQUENCE dbo.\"Trips_Id_seq\" INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 OWNED BY dbo.\"Trips\".\"Id\"");
			Sql("ALTER TABLE dbo.\"Trips\" ALTER COLUMN \"Id\" SET DEFAULT nextval('dbo.\"Trips_Id_seq\"'::regclass)");
		}

		public override void Down()
        {
        }
    }
}
