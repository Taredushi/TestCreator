namespace TestCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionsLimit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "QuestionsLimit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tests", "QuestionsLimit");
        }
    }
}
