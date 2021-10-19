namespace Lendship.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Evaluations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EvaluationAdvertisers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Flexibility = c.Int(nullable: false),
                        Reliability = c.Int(nullable: false),
                        QualityOfProduct = c.Int(nullable: false),
                        IsAnonymous = c.Boolean(nullable: false),
                        Creation = c.DateTime(nullable: false),
                        Advertisement_Id = c.Int(nullable: false),
                        UserFrom_Id = c.String(nullable: false, maxLength: 128),
                        UserTo_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserFrom_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserTo_Id)
                .Index(t => t.Advertisement_Id)
                .Index(t => t.UserFrom_Id)
                .Index(t => t.UserTo_Id);
            
            CreateTable(
                "dbo.EvaluationLenders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Flexibility = c.Int(nullable: false),
                        Reliability = c.Int(nullable: false),
                        QualityAtReturn = c.Int(nullable: false),
                        IsAnonymous = c.Boolean(nullable: false),
                        Creation = c.DateTime(nullable: false),
                        Advertisement_Id = c.Int(nullable: false),
                        UserFrom_Id = c.String(nullable: false, maxLength: 128),
                        UserTo_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserFrom_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserTo_Id)
                .Index(t => t.Advertisement_Id)
                .Index(t => t.UserFrom_Id)
                .Index(t => t.UserTo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EvaluationLenders", "UserTo_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EvaluationLenders", "UserFrom_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EvaluationLenders", "Advertisement_Id", "dbo.Advertisements");
            DropForeignKey("dbo.EvaluationAdvertisers", "UserTo_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EvaluationAdvertisers", "UserFrom_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EvaluationAdvertisers", "Advertisement_Id", "dbo.Advertisements");
            DropIndex("dbo.EvaluationLenders", new[] { "UserTo_Id" });
            DropIndex("dbo.EvaluationLenders", new[] { "UserFrom_Id" });
            DropIndex("dbo.EvaluationLenders", new[] { "Advertisement_Id" });
            DropIndex("dbo.EvaluationAdvertisers", new[] { "UserTo_Id" });
            DropIndex("dbo.EvaluationAdvertisers", new[] { "UserFrom_Id" });
            DropIndex("dbo.EvaluationAdvertisers", new[] { "Advertisement_Id" });
            DropTable("dbo.EvaluationLenders");
            DropTable("dbo.EvaluationAdvertisers");
        }
    }
}
