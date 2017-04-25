using DPTS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPTS.Web.Models
{
    public partial class BlogCommentModel : BaseEntity
    {
        public string VisitorId { get; set; }

        public string VisitorName { get; set; }

        public string VisitorAvatarUrl { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool AllowViewingProfiles { get; set; }
    }
}