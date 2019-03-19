namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        EnrollmentDate = c.DateTime(nullable: false),
                        Path = c.String(),
                        Size = c.Byte(nullable: false),
                        Category_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Autor = c.String(),
                        Text = c.String(),
                        File_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.File", t => t.File_Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.File_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Mail = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        To = c.String(),
                        From = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        Path = c.String(),
                        Category_Id = c.Int(),
                        File_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .ForeignKey("dbo.File", t => t.File_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.File_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mail", "File_Id", "dbo.File");
            DropForeignKey("dbo.Mail", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.File", "User_Id", "dbo.User");
            DropForeignKey("dbo.Comment", "User_Id", "dbo.User");
            DropForeignKey("dbo.Comment", "File_Id", "dbo.File");
            DropForeignKey("dbo.File", "Category_Id", "dbo.Category");
            DropIndex("dbo.Mail", new[] { "File_Id" });
            DropIndex("dbo.Mail", new[] { "Category_Id" });
            DropIndex("dbo.Comment", new[] { "User_Id" });
            DropIndex("dbo.Comment", new[] { "File_Id" });
            DropIndex("dbo.File", new[] { "User_Id" });
            DropIndex("dbo.File", new[] { "Category_Id" });
            DropTable("dbo.Mail");
            DropTable("dbo.User");
            DropTable("dbo.Comment");
            DropTable("dbo.File");
            DropTable("dbo.Category");
        }
    }
}
