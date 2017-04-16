namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateReviewComments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReviewComments", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ReviewComments", "Doctor_DoctorId", "dbo.Doctors");
            DropIndex("dbo.ReviewComments", new[] { "AspNetUser_Id" });
            DropIndex("dbo.ReviewComments", new[] { "Doctor_DoctorId" });
            AddColumn("dbo.ReviewComments", "CommentOwnerUser", c => c.String());
            AlterColumn("dbo.ReviewComments", "CommentForId", c => c.String());
            DropColumn("dbo.ReviewComments", "AspNetUser_Id");
            DropColumn("dbo.ReviewComments", "Doctor_DoctorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReviewComments", "Doctor_DoctorId", c => c.String(maxLength: 128));
            AddColumn("dbo.ReviewComments", "AspNetUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.ReviewComments", "CommentForId", c => c.Int(nullable: false));
            DropColumn("dbo.ReviewComments", "CommentOwnerUser");
            CreateIndex("dbo.ReviewComments", "Doctor_DoctorId");
            CreateIndex("dbo.ReviewComments", "AspNetUser_Id");
            AddForeignKey("dbo.ReviewComments", "Doctor_DoctorId", "dbo.Doctors", "DoctorId");
            AddForeignKey("dbo.ReviewComments", "AspNetUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
