namespace Lendship.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertisementAndReservation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertisements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        InstructionManual = c.String(),
                        Price = c.Int(),
                        Credit = c.Int(),
                        Deposit = c.Int(),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPublic = c.Boolean(nullable: false),
                        Creation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AdvertisementId = c.Int(nullable: false),
                        ReservationStateId = c.Int(nullable: false),
                        Comment = c.String(),
                        admittedByAdvertiser = c.Boolean(nullable: false),
                        admittedByLender = c.Boolean(nullable: false),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .ForeignKey("dbo.ReservationStates", t => t.ReservationStateId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AdvertisementId)
                .Index(t => t.ReservationStateId);
            
            CreateTable(
                "dbo.ReservationStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reservations", "ReservationStateId", "dbo.ReservationStates");
            DropForeignKey("dbo.Reservations", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.Advertisements", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reservations", new[] { "ReservationStateId" });
            DropIndex("dbo.Reservations", new[] { "AdvertisementId" });
            DropIndex("dbo.Reservations", new[] { "UserId" });
            DropIndex("dbo.Advertisements", new[] { "UserId" });
            DropTable("dbo.ReservationStates");
            DropTable("dbo.Reservations");
            DropTable("dbo.Advertisements");
        }
    }
}
