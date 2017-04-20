using DPTS.Domain.Entities;
using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class AdminDocorReviewModel : BaseEntity
    {
        [DisplayName("Doctor")]
        public string DoctorId { get; set; }

        [DisplayName("Doctor Name")]
        public string DoctorName { get; set; }

        [DisplayName("Patient")]
        public string PatientId { get; set; }
        [DisplayName("Visitor Name")]
        public string PatientInfo { get; set; }

        [AllowHtml]
        [DisplayName("Review Title")]
        public string Title { get; set; }

        [AllowHtml]
        [DisplayName("Review Text")]
        public string ReviewText { get; set; }

        [DisplayName("Rating")]
        public int Rating { get; set; }

        [DisplayName("IsApproved")]
        public bool IsApproved { get; set; }

        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }

        [AllowHtml]
        [DisplayName("Reply Text")]
        public string ReplyText { get; set; }
    }
}