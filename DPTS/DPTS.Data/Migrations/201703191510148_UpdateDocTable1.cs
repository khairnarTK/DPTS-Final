namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDocTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "IsAvailability", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "IsAvailability");
        }
    }
}
