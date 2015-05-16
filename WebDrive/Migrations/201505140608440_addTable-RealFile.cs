namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableRealFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RealFile",
                c => new
                    {
                        RealFileID = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        FileName = c.String(),
                        FileType = c.String(),
                        FileSize = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Available = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RealFileID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RealFile");
        }
    }
}
