namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDoctorTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "Language", c => c.String());
            DropColumn("dbo.Doctors", "Qualifications");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "Qualifications", c => c.String());
            DropColumn("dbo.Doctors", "Language");
        }
    }
}
