namespace TestCreator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_avatar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Avatar", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Avatar");
        }
    }
}
