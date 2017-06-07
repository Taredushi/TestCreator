namespace TestCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOfUserTestWhenTestWasDonePropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTests", "WhenTestWasDone", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTests", "WhenTestWasDone");
        }
    }
}
