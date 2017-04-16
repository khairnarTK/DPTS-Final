namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateReatingAndReview : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DoctorReviews", new[] { "Doctor_DoctorId" });
            DropColumn("dbo.DoctorReviews", "DoctorId");
            RenameColumn(table: "dbo.DoctorReviews", name: "Doctor_DoctorId", newName: "DoctorId");
            AlterColumn("dbo.DoctorReviews", "VisitorId", c => c.String());
            AlterColumn("dbo.DoctorReviews", "DoctorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.DoctorReviewHelpfulnesses", "VisitorId", c => c.String());
            CreateIndex("dbo.DoctorReviews", "DoctorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DoctorReviews", new[] { "DoctorId" });
            AlterColumn("dbo.DoctorReviewHelpfulnesses", "VisitorId", c => c.Int(nullable: false));
            AlterColumn("dbo.DoctorReviews", "DoctorId", c => c.Int(nullable: false));
            AlterColumn("dbo.DoctorReviews", "VisitorId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.DoctorReviews", name: "DoctorId", newName: "Doctor_DoctorId");
            AddColumn("dbo.DoctorReviews", "DoctorId", c => c.Int(nullable: false));
            CreateIndex("dbo.DoctorReviews", "Doctor_DoctorId");
        }
    }
}
