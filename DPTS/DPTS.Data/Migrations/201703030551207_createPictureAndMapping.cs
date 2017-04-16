namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createPictureAndMapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PictureMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        PictureId = c.Int(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId)
                .ForeignKey("dbo.Doctors", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PictureId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureBinary = c.Binary(),
                        MimeType = c.String(),
                        AltAttribute = c.String(),
                        TitleAttribute = c.String(),
                        IsNew = c.Boolean(nullable: false),
                        SeoFilename = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PictureMappings", "UserId", "dbo.Doctors");
            DropForeignKey("dbo.PictureMappings", "PictureId", "dbo.Pictures");
            DropIndex("dbo.PictureMappings", new[] { "PictureId" });
            DropIndex("dbo.PictureMappings", new[] { "UserId" });
            DropTable("dbo.Pictures");
            DropTable("dbo.PictureMappings");
        }
    }
}
