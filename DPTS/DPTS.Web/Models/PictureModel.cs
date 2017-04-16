using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPTS.Web.Models
{
    public class PictureModel
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
    }
}