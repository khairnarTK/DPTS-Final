using System;
using System.Collections.Generic;
using DPTS.Domain.Core.ReviewComments;
using DPTS.Domain.Entities;
using DPTS.Domain.Core;
using System.Linq;
using DPTS.Data.Context;

namespace DPTS.Domain.ReviewComments
{
    public class ReviewCommentsService : IReviewCommentsService
    {
        #region Fields
        private readonly IRepository<Domain.Entities.ReviewComments> _reviewComments;
        private readonly DPTSDbContext _context;
        #endregion

        #region Constructor
        public ReviewCommentsService(IRepository<Domain.Entities.ReviewComments> reviewComments)
        {
            _reviewComments = reviewComments;
            _context = new DPTSDbContext();
        }
        #endregion

        #region Methods
        IList<Entities.DoctorUserReviewComments> IReviewCommentsService.GetAllAprovedReviewCommentsByUser(string UserId)
        {
            if (string.IsNullOrWhiteSpace(UserId))
                return null;
            try
            {

                // return _reviewComments.Table.Where(c => c.IsApproved == true && c.IsActive == true && c.CommentForId==UserId).ToList();

                var query = (from r in _context.ReviewComments
                             join U1 in _context.AspNetUsers on r.CommentForId equals U1.Id
                             join U2 in _context.AspNetUsers on r.CommentOwnerId equals U2.Id
                             where r.IsActive == true && r.IsApproved == true && r.CommentForId == UserId
                             select
                              new DoctorUserReviewComments()
                              {
                                  Id = r.Id,
                                  DoctorName = U1.UserName,
                                  CommentOwnerId = r.CommentOwnerId,
                                  Username = U2.UserName,
                                  Comment = r.Comment,
                                  Rating = r.Rating,
                                  DateCreated = r.DateCreated
                              });


                return query.ToList<DoctorUserReviewComments>();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool InsertReviewComment(Entities.ReviewComments ReviewComments)
        {
            try
            {
                if (ReviewComments == null)
                    throw new ArgumentNullException(nameof(ReviewComments));

                _reviewComments.Insert(ReviewComments);

                return true;
            }
            catch {
                return false;
            }
        }

        //public decimal GetAverageScoreByUser(string UserId)
        //{
        //    from r in _reviewComments.Table
        //    where r.IsActive == true
        //        && r.IsApproved == true
        //        && r.CommentForId == UserId
        //    select r.Rating;

        //    //return _reviewComments.Table.Where(c => c.IsApproved == true && c.IsActive == true && c.CommentForId == UserId).ToList();
        //}

        //public IList<RatingDetails> GetRatingDetailsScoreByUser(string UserId)
        //{
        //    var query = (from r in _context.ReviewComments
        //                 where r.Rating==20
        //                 select new { Key = 1, Value = r.Rating })
        //}

        IList<DoctorUserReviewComments> IReviewCommentsService.GetReviewCommentsApprovalList()
        {
            try
            {
                var query = (from r in _context.ReviewComments
                             join U1 in _context.AspNetUsers on r.CommentForId equals U1.Id
                             join U2 in _context.AspNetUsers on r.CommentOwnerId equals U2.Id
                             where r.IsActive == true && r.IsApproved == false
                             select
                              new DoctorUserReviewComments()
                              {
                                  Id = r.Id
                             ,
                                  DoctorName = U1.UserName
                             ,
                                  Username = U2.UserName
                             ,
                                  Comment = r.Comment
                             ,
                                  Rating = r.Rating
                              });


                return query.ToList<DoctorUserReviewComments>();
                //return _reviewComments.Table.Where(c => c.IsApproved == false && c.IsActive == true).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        Entities.ReviewComments IReviewCommentsService.GetReviewCommentById(int id)
        {
            try
            {
                return _reviewComments.GetById(id);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool DeleteReviewComment(Entities.ReviewComments ReviewComment)
        {
            try
            {
                _reviewComments.Delete(ReviewComment);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ApproveReviewComment(int id)
        {
            try
            {
                Entities.ReviewComments reviewComments = _reviewComments.GetById(id);
                reviewComments.IsApproved = true;
                _reviewComments.Update(reviewComments);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
