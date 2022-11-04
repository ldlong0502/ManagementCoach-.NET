using ManagementCoach.BE.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(File.ReadLines("connection-string.txt").First());
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CoachSeat>()
						.Property(p => p.Status).HasDefaultValue("NotTaken");
			modelBuilder.Entity<CoachSeat>()
						.HasIndex(p => p.Name)
						.IsUnique();

			modelBuilder.Entity<Coach>()
						.Property(p => p.Status).HasDefaultValue("Active");
			modelBuilder.Entity<Coach>()
						.HasIndex(p => p.RegNo)
						.IsUnique();

			modelBuilder.Entity<Trip>()
						.Property(p => p.Cancelled).HasDefaultValue(false);

			modelBuilder.Entity<Province>()
						.HasIndex(p => p.Name)
						.IsUnique();

			modelBuilder.Entity<Passenger>()
						.Property(p => p.Blocked).HasDefaultValue(false);
			modelBuilder.Entity<Passenger>()
						.HasIndex(p => p.Phone)
						.IsUnique();
			modelBuilder.Entity<Passenger>()
						.HasIndex(p => p.Email)
						.IsUnique();

			modelBuilder.Entity<Driver>()
						.Property(p => p.Active).HasDefaultValue(true);
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

			modelBuilder.Entity<Station>()
						.HasIndex(p => p.District)
						.IsUnique();

			modelBuilder.Entity<User>()
				.HasIndex(p => p.Username)
				.IsUnique();
			modelBuilder.Entity<User>()
				.HasIndex(p => p.Email)
				.IsUnique();
		}
	}
}
