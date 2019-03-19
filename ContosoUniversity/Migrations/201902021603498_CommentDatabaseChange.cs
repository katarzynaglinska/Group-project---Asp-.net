namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentDatabaseChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "User_Id", "dbo.User");
            DropForeignKey("dbo.Comment", "File_Id", "dbo.File");
            DropIndex("dbo.Comment", new[] { "File_Id" });
            DropIndex("dbo.Comment", new[] { "User_Id" });
            AlterColumn("dbo.Comment", "File_Id", c => c.Int());
            CreateIndex("dbo.Comment", "File_Id");
            AddForeignKey("dbo.Comment", "File_Id", "dbo.File", "Id");
            DropColumn("dbo.Comment", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comment", "User_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Comment", "File_Id", "dbo.File");
            DropIndex("dbo.Comment", new[] { "File_Id" });
            AlterColumn("dbo.Comment", "File_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Comment", "User_Id");
            CreateIndex("dbo.Comment", "File_Id");
            AddForeignKey("dbo.Comment", "File_Id", "dbo.File", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comment", "User_Id", "dbo.User", "Id", cascadeDelete: true);
        }
    }
}
