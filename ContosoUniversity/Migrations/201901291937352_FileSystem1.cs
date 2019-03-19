namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileSystem1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entry", "Size", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entry", "Size", c => c.Byte(nullable: false));
        }
    }
}
