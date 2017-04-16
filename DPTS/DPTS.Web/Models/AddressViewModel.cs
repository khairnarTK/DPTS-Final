using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class AddressViewModel
    {
        public AddressViewModel()
        {
            AvailableCountry = new List<SelectListItem>();
            AvailableStateProvince = new List<SelectListItem>();
        }
        public IList<SelectListItem> AvailableCountry { get; set; }
        public IList<SelectListItem> AvailableStateProvince { get; set; }

        public int Id { get; set; }
        //Address
        [Required]
        [Display(Name = "Hospital Name")]
        public string Hospital { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateProvinceId { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "Zip /Postal Code")]
        public string ZipPostalCode { get; set; }


        [Display(Name = "Landline Number")]
        public string LandlineNumber { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Fax")]
        public string FaxNumber { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }
    }
}