namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHonorsAwardsTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HonorsAwards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        AwardDate = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HonorsAwards", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.HonorsAwards", new[] { "DoctorId" });
            DropTable("dbo.HonorsAwards");
        }
    }
}
