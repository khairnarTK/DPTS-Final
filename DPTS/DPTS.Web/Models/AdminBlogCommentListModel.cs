using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class AdminBlogCommentListModel
    {
        public AdminBlogCommentListModel()
        {
            AvailableApprovedOptions = new List<SelectListItem>();
        }

        [DisplayName("CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [DisplayName("CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [DisplayName("SearchText")]
        [AllowHtml]
        public string SearchText { get; set; }

        [DisplayName("SearchApproved")]
        public int SearchApprovedId { get; set; }

        public IList<SelectListItem> AvailableApprovedOptions { get; set; }
    }
}