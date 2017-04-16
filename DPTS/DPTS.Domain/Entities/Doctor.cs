using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace DPTS.Domain.Entities
{
    public partial class Doctor :BaseEntityWithDateTime
    {
        public Doctor()
        {
            SpecialityMapping = new HashSet<SpecialityMapping>();
            AddresseMapping = new HashSet<AddressMapping>();
            AppointmentSchedules = new HashSet<AppointmentSchedule>();
            Schedules = new HashSet<Schedule>();
            SocialLinkInformation =new HashSet<SocialLinkInformation>();
            HonorsAwards = new HashSet<HonorsAwards>();
            Education =new HashSet<Education>();
            Experience = new HashSet<Experience>();
            PictureMapping = new HashSet<PictureMapping>();
        }

        public Guid DoctorGuid { get; set; }

        public string Gender { get; set; }

        public string MiddleName { get; set; }

        public string Language { get; set; }

        public string RegistrationNumber { get; set; }

        public string ShortProfile { get; set; }

        public string Expertise { get; set; }

        public string Subscription { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public int DisplayOrder { get; set; }

        [Key]
        public string DoctorId { get; set; }

        public string DateOfBirth { get; set; }

        public decimal Rating { get; set; }

        public string ProfessionalStatements { get; set; }

        public string VideoLink { get; set; }

        public decimal ConsultationFee { get; set; }

        public bool IsAvailability { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual ICollection<SpecialityMapping> SpecialityMapping { get; set; }

        public virtual ICollection<AddressMapping> AddresseMapping { get; set; }

        public virtual ICollection<AppointmentSchedule> AppointmentSchedules { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual ICollection<SocialLinkInformation> SocialLinkInformation { get; set; }

        public virtual ICollection<HonorsAwards> HonorsAwards { get; set; }

        public virtual ICollection<Education> Education { get; set; }

        public virtual ICollection<Experience> Experience { get; set; }

        public virtual ICollection<PictureMapping> PictureMapping { get; set; }
    }
}
