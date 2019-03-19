namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "File_Id", "dbo.File");
            DropForeignKey("dbo.Comment", "User_Id", "dbo.User");
            DropIndex("dbo.Comment", new[] { "File_Id" });
            DropIndex("dbo.Comment", new[] { "User_Id" });
            AlterColumn("dbo.Comment", "File_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Comment", "User_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Comment", "File_Id");
            CreateIndex("dbo.Comment", "User_Id");
            AddForeignKey("dbo.Comment", "File_Id", "dbo.File", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comment", "User_Id", "dbo.User", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "User_Id", "dbo.User");
            DropForeignKey("dbo.Comment", "File_Id", "dbo.File");
            DropIndex("dbo.Comment", new[] { "User_Id" });
            DropIndex("dbo.Comment", new[] { "File_Id" });
            AlterColumn("dbo.Comment", "User_Id", c => c.Int());
            AlterColumn("dbo.Comment", "File_Id", c => c.Int());
            CreateIndex("dbo.Comment", "User_Id");
            CreateIndex("dbo.Comment", "File_Id");
            AddForeignKey("dbo.Comment", "User_Id", "dbo.User", "Id");
            AddForeignKey("dbo.Comment", "File_Id", "dbo.File", "Id");
        }
    }
}
