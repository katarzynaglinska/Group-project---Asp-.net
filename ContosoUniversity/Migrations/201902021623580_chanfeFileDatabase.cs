namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chanfeFileDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "File_Id", "dbo.File");
            DropIndex("dbo.Comment", new[] { "File_Id" });
            DropColumn("dbo.Comment", "File_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comment", "File_Id", c => c.Int());
            CreateIndex("dbo.Comment", "File_Id");
            AddForeignKey("dbo.Comment", "File_Id", "dbo.File", "Id");
        }
    }
}
