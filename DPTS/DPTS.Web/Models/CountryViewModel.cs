using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class CountryViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(2, ErrorMessage = "only 2 letter allow")]
        [MaxLength(2, ErrorMessage = "only 2 letter allow")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Two letter ISO code",Description = "The two letter ISO code for this country. For a complete list of ISO codes go to: http://en.wikipedia.org/wiki/ISO_3166-1_alpha.")]
        public string TwoLetterIsoCode { get; set; }

        [Required]
        [AllowHtml]
        [MinLength(3,ErrorMessage = "only 3 letter allow")]
        [MaxLength(3,ErrorMessage = "only 3 letter allow")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "Three Letter Iso Code",Description = "The three letter ISO code for this country. For a complete list of ISO codes go to: http://en.wikipedia.org/wiki/ISO_3166-1_alpha-3")]
        public string ThreeLetterIsoCode { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]

        [Display(Name= "Numeric Iso Code", Description = "The numeric ISO code for this country. For a complete list of ISO codes go to: http://en.wikipedia.org/wiki/ISO_3166-1_numeric")]
        public int NumericIsoCode { get; set; }

        [Display(Name = "Subject To Vat",Description = "Value indicating whether customers in this country must be charged EU VAT (the European Union Value Added Tax).")]
        public bool SubjectToVat { get; set; }

        [Display(Name = "Published")]
        public bool Published { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }
    }
}