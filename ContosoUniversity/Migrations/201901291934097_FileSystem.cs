namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileSystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        EnrollmentDate = c.DateTime(nullable: false),
                        Path = c.String(),
                        Size = c.Byte(nullable: false),
                        Type = c.String(),
                        ParentEntry_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entry", t => t.ParentEntry_Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.ParentEntry_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Comment", "Entry_Id", c => c.Int());
            CreateIndex("dbo.Comment", "Entry_Id");
            AddForeignKey("dbo.Comment", "Entry_Id", "dbo.Entry", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entry", "User_Id", "dbo.User");
            DropForeignKey("dbo.Entry", "ParentEntry_Id", "dbo.Entry");
            DropForeignKey("dbo.Comment", "Entry_Id", "dbo.Entry");
            DropIndex("dbo.Entry", new[] { "User_Id" });
            DropIndex("dbo.Entry", new[] { "ParentEntry_Id" });
            DropIndex("dbo.Comment", new[] { "Entry_Id" });
            DropColumn("dbo.Comment", "Entry_Id");
            DropTable("dbo.Entry");
        }
    }
}
