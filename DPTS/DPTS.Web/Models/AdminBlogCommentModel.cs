using DPTS.Domain.Entities;
using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public partial class AdminBlogCommentModel : BaseEntity
    {
        [DisplayName("Blog Post")]
        public int BlogPostId { get; set; }
        [DisplayName("Blog Post Title")]
        [AllowHtml]
        public string BlogPostTitle { get; set; }

        [DisplayName("Customer")]
        public string VisitorId { get; set; }
        [DisplayName("Custome Info")]
        public string VisitorInfo { get; set; }

        [AllowHtml]
        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DisplayName("IsApproved")]
        public bool IsApproved { get; set; }

        [DisplayName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

    }
}