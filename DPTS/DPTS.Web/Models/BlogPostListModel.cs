using DPTS.Domain.Entities;
using System.Collections.Generic;

namespace DPTS.Web.Models
{
    public partial class BlogPostListModel : BaseEntity
    {
        public BlogPostListModel()
        {
            PagingFilteringContext = new BlogPagingFilteringModel();
            BlogPosts = new List<BlogPostModel>();
        }

        public BlogPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<BlogPostModel> BlogPosts { get; set; }
    }
}