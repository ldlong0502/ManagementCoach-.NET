using ManagementCoach.BE.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE
{
	public class CoachManContext : DbContext
	{
		public DbSet<Coach> Coaches { get; set; } 
		public DbSet<CoachSeat> CoachSeats { get; set; }
		public DbSet<Driver> Drivers { get; set; } 
		public DbSet<Passenger> Passengers { get; set; }
		public DbSet<Province> Provinces { get; set; }
		public DbSet<RestArea> RestAreas { get; set; } 
		public DbSet<Route> Routes { get; set; } 
		public DbSet<RouteRestArea> RouteRestArea { get; set; } 
		public DbSet<Station> Stations { get; set; } 
		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<Trip> Trips { get; set; } 
		public DbSet<User> Users { get; set; } 

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Coach>()
						.HasIndex(p => p.RegNo)
						.IsUnique();

			modelBuilder.Entity<Province>()
						.HasIndex(p => p.Name)
						.IsUnique();
			modelBuilder.Entity<Province>()
						.Property(p => p.Id)
						.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			modelBuilder.Entity<Passenger>()
						.HasIndex(p => p.Phone)
						.IsUnique();
			modelBuilder.Entity<Passenger>()
						.HasIndex(p => p.Email)
						.IsUnique();

			modelBuilder.Entity<Driver>()
						.HasIndex(p => p.Phone)
						.IsUnique();
			modelBuilder.Entity<Driver>()
						.HasIndex(p => p.License)
						.IsUnique();
			modelBuilder.Entity<Driver>()
						.HasIndex(p => p.Email)
						.IsUnique();

			modelBuilder.Entity<RestArea>()
						.HasIndex(p => p.Address)
						.IsUnique();

			modelBuilder.Entity<RouteRestArea>()
						.HasKey(p => new { p.RouteId, p.RestAreaId });


			modelBuilder.Entity<Route>()
						.HasIndex(p => new { p.OriginStationId, p.DestinationStationId }).IsUnique();


			modelBuilder.Entity<Station>()
						.HasIndex(p => p.District)
						.IsUnique();

			modelBuilder.Entity<User>()
				.HasIndex(p => p.Username)
				.IsUnique();
			modelBuilder.Entity<User>()
				.HasIndex(p => p.Email)
				.IsUnique();

			//modelBuilder.Entity<Trip>().HasRequired(r => r.Route)
			//	.WithMany(p => p.Trips);
		}
	}
}
