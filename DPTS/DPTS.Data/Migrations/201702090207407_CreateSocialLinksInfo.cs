namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSocialLinksInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SocialLinkInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        SocialType = c.String(nullable: false),
                        SocialLink = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialLinkInformations", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.SocialLinkInformations", new[] { "DoctorId" });
            DropTable("dbo.SocialLinkInformations");
        }
    }
}
