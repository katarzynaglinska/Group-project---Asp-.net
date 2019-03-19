namespace Uploader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeCommentModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "Entry_Id", "dbo.Entry");
            DropIndex("dbo.Comment", new[] { "Entry_Id" });
            RenameColumn(table: "dbo.Comment", name: "Entry_Id", newName: "EntryId");
            AlterColumn("dbo.Comment", "EntryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comment", "EntryId");
            AddForeignKey("dbo.Comment", "EntryId", "dbo.Entry", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "EntryId", "dbo.Entry");
            DropIndex("dbo.Comment", new[] { "EntryId" });
            AlterColumn("dbo.Comment", "EntryId", c => c.Int());
            RenameColumn(table: "dbo.Comment", name: "EntryId", newName: "Entry_Id");
            CreateIndex("dbo.Comment", "Entry_Id");
            AddForeignKey("dbo.Comment", "Entry_Id", "dbo.Entry", "Id");
        }
    }
}
