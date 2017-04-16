namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentAndReview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientId = c.String(maxLength: 128),
                        DoctorId = c.String(maxLength: 128),
                        IsApproved = c.Boolean(nullable: false),
                        Title = c.String(),
                        ReviewText = c.String(),
                        ReplyText = c.String(),
                        Rating = c.Int(nullable: false),
                        HelpfulYesTotal = c.Int(nullable: false),
                        HelpfulNoTotal = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.PatientReviewHelpfulnesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorReviewId = c.Int(nullable: false),
                        WasHelpful = c.Boolean(nullable: false),
                        PatientId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DoctorReviews", t => t.DoctorReviewId, cascadeDelete: true)
                .Index(t => t.DoctorReviewId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientReviewHelpfulnesses", "DoctorReviewId", "dbo.DoctorReviews");
            DropForeignKey("dbo.DoctorReviews", "PatientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DoctorReviews", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.PatientReviewHelpfulnesses", new[] { "DoctorReviewId" });
            DropIndex("dbo.DoctorReviews", new[] { "DoctorId" });
            DropIndex("dbo.DoctorReviews", new[] { "PatientId" });
            DropTable("dbo.PatientReviewHelpfulnesses");
            DropTable("dbo.DoctorReviews");
        }
    }
}
