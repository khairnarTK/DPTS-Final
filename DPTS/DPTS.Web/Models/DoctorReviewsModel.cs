using DPTS.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    //public class DoctorReviewsModel
    //{
    //}
    public partial class DoctorReviewsModel : BaseEntity
    {
        public DoctorReviewsModel()
        {
            Items = new List<DoctorReviewModel>();
            AddDoctorReview = new AddDoctorReviewModel();
        }
        public string DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string DoctorSeName { get; set; }

        public IList<DoctorReviewModel> Items { get; set; }
        public AddDoctorReviewModel AddDoctorReview { get; set; }
    }
    public partial class DoctorReviewModel : BaseEntity
    {
        public string PatientId { get; set; }

        public string PatientName { get; set; }

        public bool AllowViewingProfiles { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public DoctorReviewHelpfulnessModel Helpfulness { get; set; }

        public string WrittenOnStr { get; set; }
    }
    public partial class DoctorReviewHelpfulnessModel : BaseEntity
    {
        public int DoctorReviewId { get; set; }

        public int HelpfulYesTotal { get; set; }

        public int HelpfulNoTotal { get; set; }
    }
    public partial class AddDoctorReviewModel : BaseEntity
    {
        [AllowHtml]
        [DisplayName("Title")]
        public string Title { get; set; }

        [AllowHtml]
        [DisplayName("Text")]
        public string ReviewText { get; set; }

        [DisplayName("Rating")]
        public int Rating { get; set; }

     //   public bool DisplayCaptcha { get; set; }

        public bool CanCurrentPatientLeaveReview { get; set; }
        public bool SuccessfullyAdded { get; set; }
        public string Result { get; set; }
    }
}