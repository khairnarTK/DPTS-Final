using System;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    public partial class AppointmentSchedule :BaseEntityWithDateTime
    {

        [Required]
        [StringLength(128)]
        public string DoctorId { get; set; }

        [Required]
        [StringLength(128)]
        public string PatientId { get; set; }

        public string Subject { get; set; }

        public string DiseasesDescription { get; set; }

        public int StatusId { get; set; }

        public string AppointmentTime { get; set; }

        public string AppointmentDate { get; set; }

        public virtual AppointmentStatus AppointmentStatus { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}