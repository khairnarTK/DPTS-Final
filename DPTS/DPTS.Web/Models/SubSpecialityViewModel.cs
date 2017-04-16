using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class SubSpecialityViewModel
    {
        public SubSpecialityViewModel()
        {
            AvailableSpeciality = new List<SelectListItem>();
        }

        public int Id { get; set; }

        public int SpecialityId { get; set; }

        public string SpecialityName { get; set; }

         [Required(ErrorMessage = "Please enter {0}")]
        //[MaxLength(256, ErrorMessage = "Maximum allowed character length   for {0} is {1}")]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }

        public IList<SelectListItem> AvailableSpeciality { get; set; }


    }
}