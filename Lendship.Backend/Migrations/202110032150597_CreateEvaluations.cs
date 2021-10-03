namespace Lendship.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateEvaluations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EvaluationAdvertisers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserFromId = c.String(nullable: false, maxLength: 128),
                        UserToId = c.String(nullable: false, maxLength: 128),
                        AdvertisementId = c.Int(nullable: false),
                        Flexibility = c.Int(nullable: false),
                        Reliability = c.Int(nullable: false),
                        QualityOfProduct = c.Int(nullable: false),
                        IsAnonymous = c.Boolean(nullable: false),
                        Creation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserFromId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserToId)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId)
                .Index(t => t.AdvertisementId);
            
            CreateTable(
                "dbo.EvaluationLenders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserFromId = c.String(nullable: false, maxLength: 128),
                        UserToId = c.String(nullable: false, maxLength: 128),
                        AdvertisementId = c.Int(nullable: false),
                        Flexibility = c.Int(nullable: false),
                        Reliability = c.Int(nullable: false),
                        QualityAtReturn = c.Int(nullable: false),
                        IsAnonymous = c.Boolean(nullable: false),
                        Creation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserFromId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserToId)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId)
                .Index(t => t.AdvertisementId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EvaluationLenders", "UserToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EvaluationLenders", "UserFromId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EvaluationLenders", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.EvaluationAdvertisers", "UserToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EvaluationAdvertisers", "UserFromId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EvaluationAdvertisers", "AdvertisementId", "dbo.Advertisements");
            DropIndex("dbo.EvaluationLenders", new[] { "AdvertisementId" });
            DropIndex("dbo.EvaluationLenders", new[] { "UserToId" });
            DropIndex("dbo.EvaluationLenders", new[] { "UserFromId" });
            DropIndex("dbo.EvaluationAdvertisers", new[] { "AdvertisementId" });
            DropIndex("dbo.EvaluationAdvertisers", new[] { "UserToId" });
            DropIndex("dbo.EvaluationAdvertisers", new[] { "UserFromId" });
            DropTable("dbo.EvaluationLenders");
            DropTable("dbo.EvaluationAdvertisers");
        }
    }
}
