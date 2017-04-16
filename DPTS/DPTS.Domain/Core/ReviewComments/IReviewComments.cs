using DPTS.Domain.Entities;
using System.Collections.Generic;
namespace DPTS.Domain.Core.ReviewComments
{
    public interface IReviewCommentsService
    {
        IList<Entities.DoctorUserReviewComments> GetAllAprovedReviewCommentsByUser(string UserId);
        //decimal GetAverageScoreByUser(string UserId);
        //IList<Entities.RatingDetails> GetRatingDetailsScoreByUser(string UserId);
        bool InsertReviewComment(Entities.ReviewComments ReviewComments);

        IList<DoctorUserReviewComments> GetReviewCommentsApprovalList();

        Entities.ReviewComments GetReviewCommentById(int id);
        
        bool DeleteReviewComment(Entities.ReviewComments ReviewComment);

        bool ApproveReviewComment(int id);
    }
}
