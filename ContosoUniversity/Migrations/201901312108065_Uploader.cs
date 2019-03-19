namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Uploader : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entry", "Size", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entry", "Size", c => c.Binary());
        }
    }
}