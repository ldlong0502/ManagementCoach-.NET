namespace ManagementCoach.Migrations
{
	using ManagementCoach.BE.Entities;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ManagementCoach.BE.CoachManContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ManagementCoach.BE.CoachManContext context)
        {
			context.Users.AddOrUpdate(x => x.Id,
				new User() { Id = 1, Name = "Admin", Username="admin", Password = MD5Helper.Encrypt("admin"), Role= "Admin" },
				new User() { Id = 2, Name = "Employee", Username="employee", Password = MD5Helper.Encrypt("employee"), Role="Employee" }
			);
		}
    }
}
