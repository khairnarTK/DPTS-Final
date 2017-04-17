using DPTS.Domain.Entities;
using DPTS.Web.AppInfra;
using System.Collections.Generic;

namespace DPTS.Web.Models
{
    //public class PatientDoctorReviewModel
    //{
    //}
    public class PatientDoctorReviewModel : BaseEntity
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSeName { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public string WrittenOnStr { get; set; }
        public string ApprovalStatus { get; set; }
    }
    public class PatientDoctorReviewsModel : BaseEntity
    {
        public PatientDoctorReviewsModel()
        {
            DoctorReviews = new List<PatientDoctorReviewModel>();
        }

        public IList<PatientDoctorReviewModel> DoctorReviews { get; set; }
        public PagerModel PagerModel { get; set; }

        #region Nested class

        /// <summary>
        /// Class that has only page for route value. Used for (My Account) My Doctor Reviews pagination
        /// </summary>
        public partial class PatientDoctorReviewsRouteValues : IRouteValues
        {
            public int page { get; set; }
        }

        #endregion
    }
}