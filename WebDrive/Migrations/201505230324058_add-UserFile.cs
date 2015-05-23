namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFile",
                c => new
                    {
                        UserFileID = c.Int(nullable: false, identity: true),
                        RealFileID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        FileName = c.String(),
                        FileType = c.String(),
                        ParentFileID = c.Int(nullable: false),
                        Directory = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserFileID)
                .ForeignKey("dbo.RealFile", t => t.RealFileID, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserID, cascadeDelete: true)
                .Index(t => t.RealFileID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserFile", new[] { "UserID" });
            DropIndex("dbo.UserFile", new[] { "RealFileID" });
            DropForeignKey("dbo.UserFile", "UserID", "dbo.UserProfile");
            DropForeignKey("dbo.UserFile", "RealFileID", "dbo.RealFile");
            DropTable("dbo.UserFile");
        }
    }
}
