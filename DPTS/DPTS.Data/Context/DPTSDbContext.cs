using System.Data.Entity;
using DPTS.Domain.Entities;
using DPTS.Domain.Entities.Notification;

namespace DPTS.Data.Context
{
    public class DPTSDbContext : DbContext
    {
        public DPTSDbContext() : base("DPTS")
        {

        }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AddressMapping> AddressMappings { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<SpecialityMapping> SpecialityMapping { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Speciality> Specialities { get; set; }
        public virtual DbSet<StateProvince> StateProvinces { get; set; }
        public virtual DbSet<SubSpeciality> SubSpecialities { get; set; }
        public virtual DbSet<AppointmentSchedule> AppointmentSchedules { get; set; }
        public virtual DbSet<AppointmentStatus> AppointmentStatus { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<EmailCategory> EmailCategory { get; set; }
        public virtual DbSet<DefaultNotificationSettings> DefaultNotificationSettings { get; set; }
        public virtual DbSet<DoctorNotificationSettings> DoctorNotificationSettings { get; set; }
        public virtual DbSet<SentEmailHistory> SentEmailHistory { get; set; }
        public virtual DbSet<SentSmsHistory> SentSmsHistory { get; set; }
        public virtual DbSet<ZipCodes> ZipCodes { get; set; }
        public virtual DbSet<SocialLinkInformation> SocialLinkInformation { get; set; }
        public virtual DbSet<HonorsAwards> HonorsAwards { get; set; }
        public virtual DbSet<Education> Education { get; set; }
        public virtual DbSet<Experience> Experience { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<PictureMapping> PictureMapping { get; set; }
        public virtual DbSet<Qualifiaction> Qualifiaction { get; set; }
        public virtual DbSet<DoctorReview> DoctorReview { get; set; }
        public virtual DbSet<PatientReviewHelpfulness> PatientReviewHelpfulness { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasMany(e => e.AddressMappings)
                .WithRequired(e => e.Address)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AddressMappings)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasOptional(e => e.Doctor)
                .WithRequired(e => e.AspNetUser);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.SpecialityMapping)
                .WithRequired(e => e.Doctor)
                .HasForeignKey(e => e.Doctor_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Speciality>()
                .HasMany(e => e.SpecialityMapping)
                .WithRequired(e => e.Speciality)
                .HasForeignKey(e => e.Speciality_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppointmentStatus>()
               .HasMany(e => e.AppointmentSchedules)
               .WithRequired(e => e.AppointmentStatus)
               .HasForeignKey(e => e.StatusId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
              .HasMany(e => e.AppointmentSchedules)
              .WithRequired(e => e.AspNetUser)
              .HasForeignKey(e => e.PatientId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
               .HasMany(e => e.AppointmentSchedules)
               .WithRequired(e => e.Doctor)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
              .HasMany(e => e.Schedules)
              .WithRequired(e => e.Doctor)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
               .HasMany(e => e.SocialLinkInformation)
               .WithRequired(e => e.Doctor)
               .HasForeignKey(e => e.DoctorId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.PictureMapping)
                .WithRequired(e => e.Doctor)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Picture>()
                .HasMany(e => e.PictureMapping)
                .WithRequired(e => e.Picture)
                .HasForeignKey(e => e.PictureId)
                .WillCascadeOnDelete(false);

        }
    }
}
