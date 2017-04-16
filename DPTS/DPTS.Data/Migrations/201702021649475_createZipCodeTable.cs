namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createZipCodeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ZipCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ZipCode = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ZipCodes");
        }
    }
}
