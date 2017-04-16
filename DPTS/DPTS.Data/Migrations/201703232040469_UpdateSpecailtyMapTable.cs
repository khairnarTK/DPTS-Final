namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSpecailtyMapTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecialityMappings", "SubSpeciality_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpecialityMappings", "SubSpeciality_Id");
        }
    }
}
