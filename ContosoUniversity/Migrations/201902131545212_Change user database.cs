namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changeuserdatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Roles", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Roles");
        }
    }
}
