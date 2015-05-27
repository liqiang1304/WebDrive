namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnSharefileNamefileType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Share", "fileName", c => c.String());
            AddColumn("dbo.Share", "fileType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Share", "fileType");
            DropColumn("dbo.Share", "fileName");
        }
    }
}
