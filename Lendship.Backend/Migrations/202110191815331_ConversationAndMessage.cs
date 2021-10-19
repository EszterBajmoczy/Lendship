namespace Lendship.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConversationAndMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Advertisement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id)
                .Index(t => t.Advertisement_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        message = c.String(nullable: false),
                        date = c.DateTime(nullable: false),
                        UserFrom_Id = c.String(nullable: false, maxLength: 128),
                        Conversation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserFrom_Id)
                .ForeignKey("dbo.Conversations", t => t.Conversation_Id)
                .Index(t => t.UserFrom_Id)
                .Index(t => t.Conversation_Id);
            
            AddColumn("dbo.AspNetUsers", "Conversation_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Conversation_Id");
            AddForeignKey("dbo.AspNetUsers", "Conversation_Id", "dbo.Conversations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Conversation_Id", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "Conversation_Id", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "UserFrom_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "Advertisement_Id", "dbo.Advertisements");
            DropIndex("dbo.Messages", new[] { "Conversation_Id" });
            DropIndex("dbo.Messages", new[] { "UserFrom_Id" });
            DropIndex("dbo.Conversations", new[] { "Advertisement_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Conversation_Id" });
            DropColumn("dbo.AspNetUsers", "Conversation_Id");
            DropTable("dbo.Messages");
            DropTable("dbo.Conversations");
        }
    }
}
