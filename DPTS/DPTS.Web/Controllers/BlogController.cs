using DPTS.Domain.Common;
using DPTS.Domain.Entities;
using DPTS.Web.AppInfra;
using DPTS.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    public partial class BlogController : BaseController
    {
        #region Fields

        private readonly IBlogService _blogService;
        private readonly IBlogModelFactory _blogModelFactory;

        #endregion

        #region Constructors

        public BlogController(IBlogService blogService, IBlogModelFactory blogModelFactory)
        {
            this._blogService = blogService;
            _blogModelFactory = blogModelFactory;
        }

        #endregion

        #region Methods

        public virtual ActionResult List(BlogPagingFilteringModel command)
        {
            var model = _blogModelFactory.PrepareBlogPostListModel(command);
            return View("List", model);
        }
        public virtual ActionResult BlogByTag(BlogPagingFilteringModel command)
        {
            var model = _blogModelFactory.PrepareBlogPostListModel(command);
            return View("List", model);
        }
        public virtual ActionResult BlogByMonth(BlogPagingFilteringModel command)
        {
            var model = _blogModelFactory.PrepareBlogPostListModel(command);
            return View("List", model);
        }

        //public virtual ActionResult ListRss(int languageId)
        //{
        //    var feed = new SyndicationFeed(
        //        string.Format("{0}: Blog", _storeContext.CurrentStore.GetLocalized(x => x.Name)),
        //        "Blog",
        //        new Uri(_webHelper.GetStoreLocation(false)),
        //        string.Format("urn:store:{0}:blog", _storeContext.CurrentStore.Id),
        //        DateTime.UtcNow);

        //    if (!_blogSettings.Enabled)
        //        return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));

        //    var items = new List<SyndicationItem>();
        //    var blogPosts = _blogService.GetAllBlogPosts(_storeContext.CurrentStore.Id, languageId);
        //    foreach (var blogPost in blogPosts)
        //    {
        //        string blogPostUrl = Url.RouteUrl("BlogPost", new { SeName = blogPost.GetSeName(blogPost.LanguageId, ensureTwoPublishedLanguages: false) }, _webHelper.IsCurrentConnectionSecured() ? "https" : "http");
        //        items.Add(new SyndicationItem(blogPost.Title, blogPost.Body, new Uri(blogPostUrl), String.Format("urn:store:{0}:blog:post:{1}", _storeContext.CurrentStore.Id, blogPost.Id), blogPost.CreatedOnUtc));
        //    }
        //    feed.Items = items;
        //    return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));
        //}

        public virtual ActionResult BlogPost(int blogPostId)
        {
            var blogPost = _blogService.GetBlogPostById(blogPostId);
            if (blogPost == null ||
                (blogPost.StartDateUtc.HasValue && blogPost.StartDateUtc.Value >= DateTime.UtcNow) ||
                (blogPost.EndDateUtc.HasValue && blogPost.EndDateUtc.Value <= DateTime.UtcNow))
                return RedirectToRoute("HomePage");
         
            var model = new BlogPostModel();
            _blogModelFactory.PrepareBlogPostModel(model, blogPost, true);

            return View(model);
        }

        [HttpPost, ActionName("BlogPost")]
      //  [FormValueRequired("add-comment")]
        public virtual ActionResult BlogCommentAdd(int blogPostId, BlogPostModel model, bool captchaValid)
        {
            var blogPost = _blogService.GetBlogPostById(blogPostId);
            if (blogPost == null || !blogPost.AllowComments)
                return RedirectToRoute("HomePage");

            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("","Blog.Comments.OnlyRegisteredUsersLeaveComments");
            }

            if (ModelState.IsValid)
            {
                var comment = new BlogComment
                {
                    BlogPostId = blogPost.Id,
                    VisitorId = User.Identity.GetUserId(),
                    CommentText = model.AddNewComment.CommentText,
                    IsApproved = true,
                    CreatedOnUtc = DateTime.UtcNow,
                };
                blogPost.BlogComments.Add(comment);
                _blogService.UpdateBlogPost(blogPost);

                //The text boxes should be cleared after a comment has been posted
                //That' why we reload the page
                TempData["nop.blog.addcomment.result"] = comment.IsApproved
                    ? "Blog.Comments.SuccessfullyAdded"
                    : "Blog.Comments.SeeAfterApproving";

               // return RedirectToRoute("BlogPost", new { SeName = blogPost.GetSeName(blogPost.LanguageId, ensureTwoPublishedLanguages: false) });
            }

            //If we got this far, something failed, redisplay form
            _blogModelFactory.PrepareBlogPostModel(model, blogPost, true);
            return View(model);
        }

        [ChildActionOnly]
        public virtual ActionResult BlogTags()
        {
            var model = _blogModelFactory.PrepareBlogPostTagListModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public virtual ActionResult BlogMonths()
        {
            var model = _blogModelFactory.PrepareBlogPostYearModel();
            return PartialView(model);
        }

        //[ChildActionOnly]
        //public virtual ActionResult RssHeaderLink()
        //{
        //    string link = string.Format("<link href=\"{0}\" rel=\"alternate\" type=\"{1}\" title=\"{2}: Blog\" />",
        //        Url.RouteUrl("BlogRSS", new { languageId = _workContext.WorkingLanguage.Id }, _webHelper.IsCurrentConnectionSecured() ? "https" : "http"), MimeTypes.ApplicationRssXml, _storeContext.CurrentStore.GetLocalized(x => x.Name));

        //    return Content(link);
        //}
        #endregion
    }
}