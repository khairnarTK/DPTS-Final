using System.ComponentModel.DataAnnotations;

namespace DPTS.Web.Models
{
    public class SpecialityViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter {0}")]
        [MaxLength(256, ErrorMessage = "Maximum allowed character length for {0} is {1}")]
        public string Title { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}