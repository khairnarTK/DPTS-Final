using System;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    public partial class Education
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string DoctorId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Institute { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
