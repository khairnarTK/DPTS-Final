using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class DoctorPictureModel
    {
        public int Id { get; set; }

        public string DoctorId { get; set; }

        [Display(Name = "Picture")]
        [UIHint("Picture")]
        public int PictureId { get; set; }

        public string PictureUrl { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [AllowHtml]
        [Display(Name = "Alt")]
        public string OverrideAltAttribute { get; set; }

        [AllowHtml]
        [Display(Name = "Title")]
        public string OverrideTitleAttribute { get; set; }
    }
}