using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AddressMappings = new HashSet<AddressMapping>();
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AppointmentSchedules = new HashSet<AppointmentSchedule>();
            AspNetRoles = new HashSet<AspNetRole>();
        }

        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsEmailUnsubscribed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
        public bool IsPhoneNumberUnsubscribed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        public string LastIpAddress { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime? LastLoginDateUtc { get; set; }

        public virtual ICollection<AddressMapping> AddressMappings { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }

        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }

        public virtual ICollection<AppointmentSchedule> AppointmentSchedules { get; set; }
    }
}
