namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableShareCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShareCode",
                c => new
                    {
                        ShareID = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        Available = c.Boolean(nullable: false),
                        ValidateString = c.String(),
                    })
                .PrimaryKey(t => t.ShareID)
                .ForeignKey("dbo.Share", t => t.ShareID)
                .Index(t => t.ShareID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ShareCode", new[] { "ShareID" });
            DropForeignKey("dbo.ShareCode", "ShareID", "dbo.Share");
            DropTable("dbo.ShareCode");
        }
    }
}
