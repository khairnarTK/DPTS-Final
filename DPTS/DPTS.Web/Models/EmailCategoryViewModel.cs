using System;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Web.Models
{
    public class EmailCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Name { get; set; }
 
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