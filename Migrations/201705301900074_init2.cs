namespace TestCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Question_QuestionID", "dbo.Questions");
            DropIndex("dbo.Questions", new[] { "Question_QuestionID" });
            DropColumn("dbo.Questions", "Question_QuestionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Question_QuestionID", c => c.Int());
            CreateIndex("dbo.Questions", "Question_QuestionID");
            AddForeignKey("dbo.Questions", "Question_QuestionID", "dbo.Questions", "QuestionID");
        }
    }
}
