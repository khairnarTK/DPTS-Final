namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHonorsAwardsTbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HonorsAwards", "AwardDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HonorsAwards", "AwardDate", c => c.String());
        }
    }
}
