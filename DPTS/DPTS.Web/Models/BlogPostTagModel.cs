using DPTS.Domain.Entities;

namespace DPTS.Web.Models
{
    public partial class BlogPostTagModel : BaseEntity
    {
        public string Name { get; set; }

        public int BlogPostCount { get; set; }
    }
}