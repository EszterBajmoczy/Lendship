namespace Lendship.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCascadeTrue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Advertisements", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Availabilities", "Advertisement_Id", "dbo.Advertisements");
            AddForeignKey("dbo.Advertisements", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Availabilities", "Advertisement_Id", "dbo.Advertisements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Availabilities", "Advertisement_Id", "dbo.Advertisements");
            DropForeignKey("dbo.Advertisements", "User_Id", "dbo.AspNetUsers");
            AddForeignKey("dbo.Availabilities", "Advertisement_Id", "dbo.Advertisements", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Advertisements", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
