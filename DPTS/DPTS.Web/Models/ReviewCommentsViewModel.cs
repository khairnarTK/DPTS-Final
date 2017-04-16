using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPTS.Web.Models
{
    public class ReviewCommentsViewModel
    {
        public string Id { get; set; }
        public string DoctorName { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public string Rating { get; set; }
    }
}