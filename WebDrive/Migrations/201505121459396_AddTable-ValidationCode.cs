namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableValidationCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ValidationCode",
                c => new
                    {
                        ValidationID = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        Available = c.Boolean(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ValidationID)
                .ForeignKey("dbo.UserProfile", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            AlterColumn("dbo.UserProfile", "RegisterDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserProfile", "ExpireLoginDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserProfile", "ExpireLoginCounts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.ValidationCode", new[] { "UserID" });
            DropForeignKey("dbo.ValidationCode", "UserID", "dbo.UserProfile");
            AlterColumn("dbo.UserProfile", "ExpireLoginCounts", c => c.Int());
            AlterColumn("dbo.UserProfile", "ExpireLoginDate", c => c.DateTime());
            AlterColumn("dbo.UserProfile", "RegisterDate", c => c.DateTime());
            DropTable("dbo.ValidationCode");
        }
    }
}
