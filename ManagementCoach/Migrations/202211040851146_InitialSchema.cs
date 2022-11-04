namespace ManagementCoach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        RegNo = c.String(nullable: false),
                        Status = c.String(),
                        Notes = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.RegNo, unique: true);
            
            CreateTable(
                "dbo.CoachSeats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoachId = c.Int(nullable: false),
                        Name = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coaches", t => t.CoachId, cascadeDelete: true)
                .Index(t => t.CoachId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RouteId = c.Int(nullable: false),
                        CoachId = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Cancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coaches", t => t.CoachId, cascadeDelete: true)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.Routes", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CoachId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IdCard = c.String(nullable: false),
                        Gender = c.String(),
                        Dob = c.DateTime(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        DateJoined = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        License = c.String(),
                        Notes = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Phone, unique: true)
                .Index(t => t.License, unique: true);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginStationId = c.Int(nullable: false),
                        DestinationStationId = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        EstimatedTime = c.Int(nullable: false),
                        Station_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.Station_Id)
                .ForeignKey("dbo.Stations", t => t.DestinationStationId, cascadeDelete: true)
                .ForeignKey("dbo.Stations", t => t.OriginStationId, cascadeDelete: true)
                .Index(t => t.OriginStationId)
                .Index(t => t.DestinationStationId)
                .Index(t => t.Station_Id);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        District = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        ProvinceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provinces", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.District, unique: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.RestAreas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        ProvinceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provinces", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.Address, unique: true);
            
            CreateTable(
                "dbo.RouteRestAreas",
                c => new
                    {
                        RouteId = c.Int(nullable: false),
                        RestAreaId = c.String(nullable: false, maxLength: 128),
                        StopOrder = c.Int(nullable: false),
                        RestArea_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.RouteId, t.RestAreaId })
                .ForeignKey("dbo.RestAreas", t => t.RestArea_Id)
                .ForeignKey("dbo.Routes", t => t.RouteId, cascadeDelete: true)
                .Index(t => t.RouteId)
                .Index(t => t.RestArea_Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TripId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DateBought = c.DateTimeOffset(nullable: false, precision: 7),
                        Passenger_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Passengers", t => t.Passenger_Id)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId)
                .Index(t => t.Passenger_Id);
            
            CreateTable(
                "dbo.Passengers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IdCard = c.String(nullable: false),
                        Gender = c.String(),
                        Dob = c.DateTime(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Blocked = c.Boolean(nullable: false),
                        Notes = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Phone, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Role = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Tickets", "Passenger_Id", "dbo.Passengers");
            DropForeignKey("dbo.Trips", "Id", "dbo.Routes");
            DropForeignKey("dbo.Routes", "OriginStationId", "dbo.Stations");
            DropForeignKey("dbo.Routes", "DestinationStationId", "dbo.Stations");
            DropForeignKey("dbo.Routes", "Station_Id", "dbo.Stations");
            DropForeignKey("dbo.Stations", "ProvinceId", "dbo.Provinces");
            DropForeignKey("dbo.RouteRestAreas", "RouteId", "dbo.Routes");
            DropForeignKey("dbo.RouteRestAreas", "RestArea_Id", "dbo.RestAreas");
            DropForeignKey("dbo.RestAreas", "Id", "dbo.Provinces");
            DropForeignKey("dbo.Trips", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Trips", "CoachId", "dbo.Coaches");
            DropForeignKey("dbo.CoachSeats", "CoachId", "dbo.Coaches");
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Passengers", new[] { "Phone" });
            DropIndex("dbo.Passengers", new[] { "Email" });
            DropIndex("dbo.Tickets", new[] { "Passenger_Id" });
            DropIndex("dbo.Tickets", new[] { "TripId" });
            DropIndex("dbo.RouteRestAreas", new[] { "RestArea_Id" });
            DropIndex("dbo.RouteRestAreas", new[] { "RouteId" });
            DropIndex("dbo.RestAreas", new[] { "Address" });
            DropIndex("dbo.RestAreas", new[] { "Id" });
            DropIndex("dbo.Provinces", new[] { "Name" });
            DropIndex("dbo.Stations", new[] { "ProvinceId" });
            DropIndex("dbo.Stations", new[] { "District" });
            DropIndex("dbo.Routes", new[] { "Station_Id" });
            DropIndex("dbo.Routes", new[] { "DestinationStationId" });
            DropIndex("dbo.Routes", new[] { "OriginStationId" });
            DropIndex("dbo.Drivers", new[] { "License" });
            DropIndex("dbo.Drivers", new[] { "Phone" });
            DropIndex("dbo.Drivers", new[] { "Email" });
            DropIndex("dbo.Trips", new[] { "DriverId" });
            DropIndex("dbo.Trips", new[] { "CoachId" });
            DropIndex("dbo.Trips", new[] { "Id" });
            DropIndex("dbo.CoachSeats", new[] { "Name" });
            DropIndex("dbo.CoachSeats", new[] { "CoachId" });
            DropIndex("dbo.Coaches", new[] { "RegNo" });
            DropTable("dbo.Users");
            DropTable("dbo.Passengers");
            DropTable("dbo.Tickets");
            DropTable("dbo.RouteRestAreas");
            DropTable("dbo.RestAreas");
            DropTable("dbo.Provinces");
            DropTable("dbo.Stations");
            DropTable("dbo.Routes");
            DropTable("dbo.Drivers");
            DropTable("dbo.Trips");
            DropTable("dbo.CoachSeats");
            DropTable("dbo.Coaches");
        }
    }
}
