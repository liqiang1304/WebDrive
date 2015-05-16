namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validationup1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ValidationCode", "UserID", "dbo.UserProfile");
            DropIndex("dbo.ValidationCode", new[] { "UserID" });
            AddColumn("dbo.ValidationCode", "ValidateString", c => c.String());
            DropPrimaryKey("dbo.ValidationCode", new[] { "ValidationID" });
            AddPrimaryKey("dbo.ValidationCode", "UserID");
            AddForeignKey("dbo.ValidationCode", "UserID", "dbo.UserProfile", "UserID");
            CreateIndex("dbo.ValidationCode", "UserID");
            DropColumn("dbo.ValidationCode", "ValidationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ValidationCode", "ValidationID", c => c.Int(nullable: false, identity: true));
            DropIndex("dbo.ValidationCode", new[] { "UserID" });
            DropForeignKey("dbo.ValidationCode", "UserID", "dbo.UserProfile");
            DropPrimaryKey("dbo.ValidationCode", new[] { "UserID" });
            AddPrimaryKey("dbo.ValidationCode", "ValidationID");
            DropColumn("dbo.ValidationCode", "ValidateString");
            CreateIndex("dbo.ValidationCode", "UserID");
            AddForeignKey("dbo.ValidationCode", "UserID", "dbo.UserProfile", "UserID", cascadeDelete: true);
        }
    }
}
