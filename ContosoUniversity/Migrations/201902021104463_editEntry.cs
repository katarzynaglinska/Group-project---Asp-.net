namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editEntry : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entry", "Size", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entry", "Size", c => c.Int());
        }
    }
}
