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
        [DisplayName("Admin.Catalog.ProductReviews.Fields.Customer")]
        public string PatientInfo { get; set; }

        [AllowHtml]
        [DisplayName("Title")]
        public string Title { get; set; }

        [AllowHtml]
        [DisplayName("ReviewText")]
        public string ReviewText { get; set; }

        [DisplayName("Admin.Catalog.ProductReviews.Fields.Rating")]
        public int Rating { get; set; }

        [DisplayName("Admin.Catalog.ProductReviews.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [DisplayName("Admin.Catalog.ProductReviews.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}