using DPTS.Common.Kendoui;
using DPTS.Domain.Common;
using DPTS.Domain.Entities;
using DPTS.Services;
using DPTS.Web.AppInfra;
using DPTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    public partial class AdminBlogController : BaseController
    {
        #region Fields

        private readonly IBlogService _blogService;
        // private readonly IUrlRecordService _urlRecordService;
        private static TimeZoneInfo INDIAN_ZONE_C = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        #endregion

        #region Ctor

        public AdminBlogController(IBlogService blogService)
        {
            this._blogService = blogService;
        }
        #endregion

        #region Utilities

        #endregion

        #region Blog posts

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
          //  var model = new AdminBlogPostListModel();

            //stores
            //model.AvailableStores.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            //foreach (var store in _storeService.GetAllStores())
              //  model.AvailableStores.Add(new SelectListItem { Text = store.Name, Value = store.Id.ToString() });

            return View();
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command)
        {
            var blogPosts = _blogService.GetAllBlogPosts(null, null, command.Page - 1, command.PageSize, true);
            var gridModel = new DataSourceResult
            {
                Data = blogPosts.Select(x =>
                {
                    var m = new AdminBlogPostModel();
                    m.AllowComments = x.AllowComments;
                    m.BodyOverview = x.BodyOverview;
                    m.Tags = x.Tags;
                    m.Title = x.Title;
                    m.Id = x.Id;

                    //little performance optimization: ensure that "Body" is not returned
                    m.Body = "";
                    if (x.StartDateUtc.HasValue)
                        m.StartDate = ConvertToUserTime(x.StartDateUtc.Value, DateTimeKind.Utc);
                    if (x.EndDateUtc.HasValue)
                        m.EndDate = ConvertToUserTime(x.EndDateUtc.Value, DateTimeKind.Utc);
                    m.CreatedOn = ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                    m.ApprovedComments = _blogService.GetBlogCommentsCount(x, isApproved: true);
                    m.NotApprovedComments = _blogService.GetBlogCommentsCount(x, isApproved: false);

                    return m;
                }),
                Total = blogPosts.TotalCount
            };

            return Json(gridModel);
        }

        public virtual ActionResult Create()
        {
            var model = new AdminBlogPostModel();
            model.AllowComments = true;
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Create(AdminBlogPostModel model)
        {
            if (ModelState.IsValid)
            {
                var blogPost = new BlogPost();
                blogPost.Title = model.Title;
                blogPost.Tags = model.Tags;
                blogPost.BodyOverview = model.BodyOverview;
                blogPost.Body = model.Body;
                blogPost.AllowComments = model.AllowComments;
                blogPost.StartDateUtc = model.StartDate;
                blogPost.EndDateUtc = model.EndDate;
                blogPost.CreatedOnUtc = DateTime.UtcNow;
                _blogService.InsertBlogPost(blogPost);

                SuccessNotification("blog post added successfully");

                return RedirectToAction("List");
            }

            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            var model = new AdminBlogPostModel();
            model.Title = blogPost.Title;
            model.Tags = blogPost.Tags;
            model.BodyOverview = blogPost.BodyOverview;
            model.Body = blogPost.Body;
            model.AllowComments = blogPost.AllowComments;
            model.StartDate = blogPost.StartDateUtc;
            model.EndDate = blogPost.EndDateUtc;
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(AdminBlogPostModel model)
        {
            var blogPost = _blogService.GetBlogPostById(model.Id);
            if (blogPost == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                blogPost.Title = model.Title;
                blogPost.Tags = model.Tags;
                blogPost.BodyOverview = model.BodyOverview;
                blogPost.Body = model.Body;
                blogPost.AllowComments = model.AllowComments;
                blogPost.StartDateUtc = model.StartDate;
                blogPost.EndDateUtc = model.EndDate;
                _blogService.UpdateBlogPost(blogPost);


                SuccessNotification("blog post pdated successfully");
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            var blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            _blogService.DeleteBlogPost(blogPost);

            SuccessNotification("blog post deleted successfully.");
            return RedirectToAction("List");
        }

        #endregion

        #region Comments

        public virtual ActionResult Comments(int? filterByBlogPostId)
        {
            ViewBag.FilterByBlogPostId = filterByBlogPostId;
            var model = new AdminBlogCommentListModel();

            //"approved" property
            //0 - all
            //1 - approved only
            //2 - disapproved only
            model.AvailableApprovedOptions.Add(new SelectListItem { Text = "All", Value = "0" });
            model.AvailableApprovedOptions.Add(new SelectListItem { Text = "Approved", Value = "1" });
            model.AvailableApprovedOptions.Add(new SelectListItem { Text = "Disapproved", Value = "2" });

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Comments(int? filterByBlogPostId, DataSourceRequest command, AdminBlogCommentListModel model)
        {
            var createdOnFromValue = model.CreatedOnFrom == null ? null
                            : (DateTime?)ConvertToUtcTime(model.CreatedOnFrom.Value, INDIAN_ZONE_C);

            var createdOnToValue = model.CreatedOnTo == null ? null
                            : (DateTime?)ConvertToUtcTime(model.CreatedOnTo.Value, INDIAN_ZONE_C).AddDays(1);

            bool? approved = null;
            if (model.SearchApprovedId > 0)
                approved = model.SearchApprovedId == 1;

            var comments = _blogService.GetAllComments(null,filterByBlogPostId, approved, createdOnFromValue, createdOnToValue, model.SearchText);

            var gridModel = new DataSourceResult
            {
                Data = comments.PagedForCommand(command).Select(blogComment =>
                {
                    var commentModel = new AdminBlogCommentModel();
                    commentModel.Id = blogComment.Id;
                    commentModel.BlogPostId = blogComment.BlogPostId;
                    commentModel.BlogPostTitle = blogComment.BlogPost.Title;
                    commentModel.VisitorId = blogComment.VisitorId;
                    var customer = blogComment.Visitor;
                    commentModel.VisitorInfo = (customer != null) ? customer.Email : string.Empty;
                    commentModel.CreatedOn = ConvertToUserTime(blogComment.CreatedOnUtc, DateTimeKind.Utc);
                    commentModel.Comment = blogComment.CommentText;
                    commentModel.IsApproved = blogComment.IsApproved;

                    return commentModel;
                }),
                Total = comments.Count,
            };
            return Json(gridModel);
        }

        [HttpPost]
        public virtual ActionResult CommentUpdate(AdminBlogCommentModel model)
        {
            var comment = _blogService.GetBlogCommentById(model.Id);
            if (comment == null)
                throw new ArgumentException("No comment found with the specified id");

            var previousIsApproved = comment.IsApproved;

            comment.IsApproved = model.IsApproved;
            _blogService.UpdateBlogPost(comment.BlogPost);

            return new NullJsonResult();
        }

        public virtual ActionResult CommentDelete(int id)
        {
            var comment = _blogService.GetBlogCommentById(id);
            if (comment == null)
                throw new ArgumentException("No comment found with the specified id");

            var blogPost = comment.BlogPost;
            _blogService.DeleteBlogComment(comment);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual ActionResult DeleteSelectedComments(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                var comments = _blogService.GetBlogCommentsByIds(selectedIds.ToArray());
                var blogPosts = _blogService.GetBlogPostsByIds(comments.Select(p => p.BlogPostId).Distinct().ToArray());

                _blogService.DeleteBlogComments(comments);
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual ActionResult ApproveSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                //filter not approved comments
                var blogComments = _blogService.GetBlogCommentsByIds(selectedIds.ToArray()).Where(comment => !comment.IsApproved);

                foreach (var blogComment in blogComments)
                {
                    blogComment.IsApproved = true;
                    _blogService.UpdateBlogPost(blogComment.BlogPost);

                }
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual ActionResult DisapproveSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                //filter approved comments
                var blogComments = _blogService.GetBlogCommentsByIds(selectedIds.ToArray()).Where(comment => comment.IsApproved);

                foreach (var blogComment in blogComments)
                {
                    blogComment.IsApproved = false;
                    _blogService.UpdateBlogPost(blogComment.BlogPost);
                }
            }

            return Json(new { Result = true });
        }

        #endregion
    }
}