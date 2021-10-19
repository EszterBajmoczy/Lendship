namespace Lendship.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClosedGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClosedGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Advertisement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id)
                .Index(t => t.Advertisement_Id);
            
            AddColumn("dbo.AspNetUsers", "ClosedGroup_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ClosedGroup_Id");
            AddForeignKey("dbo.AspNetUsers", "ClosedGroup_Id", "dbo.ClosedGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ClosedGroup_Id", "dbo.ClosedGroups");
            DropForeignKey("dbo.ClosedGroups", "Advertisement_Id", "dbo.Advertisements");
            DropIndex("dbo.ClosedGroups", new[] { "Advertisement_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ClosedGroup_Id" });
            DropColumn("dbo.AspNetUsers", "ClosedGroup_Id");
            DropTable("dbo.ClosedGroups");
        }
    }
}
