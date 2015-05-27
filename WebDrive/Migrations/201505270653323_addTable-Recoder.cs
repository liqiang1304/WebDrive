namespace WebDrive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableRecoder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recoder",
                c => new
                    {
                        ReocderID = c.Int(nullable: false, identity: true),
                        RecoderString = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        ExpireTime = c.DateTime(nullable: false),
                        Available = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ReocderID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Recoder");
        }
    }
}
