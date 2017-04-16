namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedoctortablemiddlename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "MiddleName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "MiddleName");
        }
    }
}
