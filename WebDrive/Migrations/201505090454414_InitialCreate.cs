namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        NickName = c.String(),
                        RegisterDate = c.DateTime(nullable: false),
                        ExpireLoginDate = c.DateTime(nullable: false),
                        Available = c.Boolean(nullable: false),
                        LoginCounts = c.Int(nullable: false),
                        ExpireLoginCounts = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.QRCode",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        CodeString = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        Available = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.UserProfile", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.QRCode", new[] { "UserID" });
            DropForeignKey("dbo.QRCode", "UserID", "dbo.UserProfile");
            DropTable("dbo.QRCode");
            DropTable("dbo.UserProfile");
        }
    }
}
