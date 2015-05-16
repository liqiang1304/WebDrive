namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMD5RealFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RealFile", "MD5", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RealFile", "MD5");
        }
    }
}
