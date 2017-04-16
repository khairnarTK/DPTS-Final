using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class StateProvinceViewModel
    {
        public StateProvinceViewModel()
        {
            AvailableCountry = new List<SelectListItem>();
        }
        public int Id { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Abbreviation")]
        public string Abbreviation { get; set; }

        [Display(Name = "Published")]
        public bool Published { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }

        public IList<SelectListItem> AvailableCountry { get; set; }
    }
}