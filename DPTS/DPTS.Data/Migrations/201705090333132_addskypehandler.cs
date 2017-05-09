namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addskypehandler : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "SkypeHandler", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "SkypeHandler");
        }
    }
}
