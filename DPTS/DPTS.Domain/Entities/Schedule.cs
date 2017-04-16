using System;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    public partial class Schedule
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string DoctorId { get; set; }

        [Required]
        [StringLength(10)]
        public string Day { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
