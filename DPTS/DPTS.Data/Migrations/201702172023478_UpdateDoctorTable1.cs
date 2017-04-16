namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDoctorTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "ProfessionalStatements", c => c.String());
            DropColumn("dbo.Doctors", "YearsOfExperience");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "YearsOfExperience", c => c.Int());
            DropColumn("dbo.Doctors", "ProfessionalStatements");
        }
    }
}
