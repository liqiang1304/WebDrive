namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyColumnNameShare : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Share", "FileName", c => c.String());
            AlterColumn("dbo.Share", "FileType", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Share", "fileType", c => c.String());
            AlterColumn("dbo.Share", "fileName", c => c.String());
        }
    }
}
