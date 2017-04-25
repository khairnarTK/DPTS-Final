using DPTS.Domain.Entities;
using System.ComponentModel;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public partial class AddBlogCommentModel : BaseEntity
    {
        [DisplayName("Comment Text")]
        [AllowHtml]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}