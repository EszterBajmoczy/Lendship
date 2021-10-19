namespace Lendship.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Advertisements", "Category_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Advertisements", "Category_Id");
            AddForeignKey("dbo.Advertisements", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advertisements", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Advertisements", new[] { "Category_Id" });
            DropColumn("dbo.Advertisements", "Category_Id");
            DropTable("dbo.Categories");
        }
    }
}
