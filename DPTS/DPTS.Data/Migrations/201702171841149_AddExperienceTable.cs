namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExperienceTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Experiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        Organization = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Experiences", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Experiences", new[] { "DoctorId" });
            DropTable("dbo.Experiences");
        }
    }
}
