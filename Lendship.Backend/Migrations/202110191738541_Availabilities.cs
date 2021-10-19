namespace Lendship.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Availabilities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Availabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        Advertisement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id, cascadeDelete: true)
                .Index(t => t.Advertisement_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Availabilities", "Advertisement_Id", "dbo.Advertisements");
            DropIndex("dbo.Availabilities", new[] { "Advertisement_Id" });
            DropTable("dbo.Availabilities");
        }
    }
}
