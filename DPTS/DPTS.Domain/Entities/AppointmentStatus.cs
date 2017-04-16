using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    public partial class AppointmentStatus
    {
        public AppointmentStatus()
        {
            AppointmentSchedules = new HashSet<AppointmentSchedule>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<AppointmentSchedule> AppointmentSchedules { get; set; }
    }
}
