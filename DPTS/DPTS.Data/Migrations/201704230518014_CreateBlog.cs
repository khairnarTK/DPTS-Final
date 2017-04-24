namespace DPTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VisitorId = c.String(maxLength: 128),
                        CommentText = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        BlogPostId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.BlogPostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.VisitorId)
                .Index(t => t.VisitorId)
                .Index(t => t.BlogPostId);
            
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        BodyOverview = c.String(),
                        AllowComments = c.Boolean(nullable: false),
                        Tags = c.String(),
                        StartDateUtc = c.DateTime(),
                        EndDateUtc = c.DateTime(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogComments", "VisitorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BlogComments", "BlogPostId", "dbo.BlogPosts");
            DropIndex("dbo.BlogComments", new[] { "BlogPostId" });
            DropIndex("dbo.BlogComments", new[] { "VisitorId" });
            DropTable("dbo.BlogPosts");
            DropTable("dbo.BlogComments");
        }
    }
}
