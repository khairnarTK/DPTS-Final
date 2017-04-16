using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class DefaultNotificationSettingsViewModel
    {
        public DefaultNotificationSettingsViewModel()
        {
            AvailableEmailCategory = new List<SelectListItem>();
        }
        public int Id { get; set; } 
        [Display(Name = "Email Category")]
        public int EmailCategoryId { get; set; }

        public string EmailCategory { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Published")]
        public bool Published { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }


        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }

        public IList<SelectListItem> AvailableEmailCategory { get; set; }
    }
}