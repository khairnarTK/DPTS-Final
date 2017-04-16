using DPTS.Domain.Core.ReviewComments;
using DPTS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    public class ReviewCommentsController : Controller
    {
        #region Field
        private readonly IReviewCommentsService _reviewCommentsService;
        #endregion

        #region Constructor
        public ReviewCommentsController(IReviewCommentsService reviewCommentsService)
        {
            _reviewCommentsService = reviewCommentsService;
        }

        #endregion


        // GET: ReviewComments
        public ActionResult ReviewCommentsApproveList()
        {
            var model = _reviewCommentsService.GetReviewCommentsApprovalList();
            return View(model);
        }

        [HttpPost]
        public ActionResult ApproveReviewComment(int id)
        {
            if (_reviewCommentsService.ApproveReviewComment(id))
                return Content("Success");
            return Content("Error");
        }

        public ActionResult DeleteConfirmed(int id)
        {
            ReviewComments reviewComment = _reviewCommentsService.GetReviewCommentById(id);
            if (reviewComment != null)
                if (_reviewCommentsService.DeleteReviewComment(reviewComment))
                    return Content("Deleted");

            return Content("Error");
        }
    }
}