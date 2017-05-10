namespace TestCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        IsCorrect = c.Boolean(nullable: false),
                        QuestionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerID)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.QuestionID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        TestID = c.Int(nullable: false),
                        Question_QuestionID = c.Int(),
                    })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("dbo.Questions", t => t.Question_QuestionID)
                .ForeignKey("dbo.Tests", t => t.TestID, cascadeDelete: true)
                .Index(t => t.TestID)
                .Index(t => t.Question_QuestionID);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TestID);
            
            CreateTable(
                "dbo.UserTests",
                c => new
                    {
                        UserTestID = c.Int(nullable: false, identity: true),
                        Result = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        TestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserTestID)
                .ForeignKey("dbo.Tests", t => t.TestID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.TestID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Role = c.Int(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.Questions", "TestID", "dbo.Tests");
            DropForeignKey("dbo.UserTests", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserTests", "TestID", "dbo.Tests");
            DropForeignKey("dbo.Questions", "Question_QuestionID", "dbo.Questions");
            DropIndex("dbo.UserTests", new[] { "TestID" });
            DropIndex("dbo.UserTests", new[] { "UserID" });
            DropIndex("dbo.Questions", new[] { "Question_QuestionID" });
            DropIndex("dbo.Questions", new[] { "TestID" });
            DropIndex("dbo.Answers", new[] { "QuestionID" });
            DropTable("dbo.Users");
            DropTable("dbo.UserTests");
            DropTable("dbo.Tests");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
