using DPTS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class AdminBlogPostModel
    {
    }
    public partial class AdminBlogPostModel : BaseEntity
    {
        public AdminBlogPostModel()
        {
            AvailableLanguages = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }

        [DisplayName("Title")]
        [AllowHtml]
        public string Title { get; set; }

        [DisplayName("Body")]
        [AllowHtml]
        public string Body { get; set; }

        [DisplayName("Body Overview")]
        [AllowHtml]
        public string BodyOverview { get; set; }

        [DisplayName("Allow Comments")]
        public bool AllowComments { get; set; }

        [DisplayName("Tags")]
        [AllowHtml]
        public string Tags { get; set; }

        public int ApprovedComments { get; set; }
        public int NotApprovedComments { get; set; }

        [DisplayName("StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [DisplayName("EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }

        [DisplayName("CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}