namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableShare : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Share",
                c => new
                    {
                        ShareID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        RealFileID = c.Int(nullable: false),
                        SharedType = c.Int(nullable: false),
                        SharedDate = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        Password = c.String(),
                        DownloadCounts = c.Int(nullable: false),
                        ExpireCounts = c.Int(nullable: false),
                        Private = c.Boolean(nullable: false),
                        SharedQRCode = c.String(),
                        UserFile_UserFileID = c.Int(),
                    })
                .PrimaryKey(t => t.ShareID)
                .ForeignKey("dbo.UserFile", t => t.UserFile_UserFileID)
                .ForeignKey("dbo.RealFile", t => t.RealFileID, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserFile_UserFileID)
                .Index(t => t.RealFileID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Share", new[] { "UserID" });
            DropIndex("dbo.Share", new[] { "RealFileID" });
            DropIndex("dbo.Share", new[] { "UserFile_UserFileID" });
            DropForeignKey("dbo.Share", "UserID", "dbo.UserProfile");
            DropForeignKey("dbo.Share", "RealFileID", "dbo.RealFile");
            DropForeignKey("dbo.Share", "UserFile_UserFileID", "dbo.UserFile");
            DropTable("dbo.Share");
        }
    }
}
