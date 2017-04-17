namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "ApprovedRatingSum", c => c.Int(nullable: false));
            AddColumn("dbo.Doctors", "NotApprovedRatingSum", c => c.Int(nullable: false));
            AddColumn("dbo.Doctors", "ApprovedTotalReviews", c => c.Int(nullable: false));
            AddColumn("dbo.Doctors", "NotApprovedTotalReviews", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "NotApprovedTotalReviews");
            DropColumn("dbo.Doctors", "ApprovedTotalReviews");
            DropColumn("dbo.Doctors", "NotApprovedRatingSum");
            DropColumn("dbo.Doctors", "ApprovedRatingSum");
        }
    }
}
