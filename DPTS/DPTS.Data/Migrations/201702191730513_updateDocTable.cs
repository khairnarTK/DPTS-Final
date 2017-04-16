namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDocTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "VideoLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "VideoLink");
        }
    }
}
