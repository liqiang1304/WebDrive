namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCondition : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfile", "UserName", c => c.String(maxLength: 30));
            AlterColumn("dbo.UserProfile", "RegisterDate", c => c.DateTime());
            AlterColumn("dbo.UserProfile", "ExpireLoginDate", c => c.DateTime());
            AlterColumn("dbo.UserProfile", "ExpireLoginCounts", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfile", "ExpireLoginCounts", c => c.Int(nullable: false));
            AlterColumn("dbo.UserProfile", "ExpireLoginDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserProfile", "RegisterDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserProfile", "UserName", c => c.String());
        }
    }
}
