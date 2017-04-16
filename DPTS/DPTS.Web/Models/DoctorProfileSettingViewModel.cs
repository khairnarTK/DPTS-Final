using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class DoctorProfileSettingViewModel
    {
        public DoctorProfileSettingViewModel()
        {
            AvailableSpeciality = new List<SelectListItem>();
            //  AvailableCountry = new List<SelectListItem>();
            //AvailableStateProvince = new List<SelectListItem>();
            SelectedSpeciality = new List<string>();
        }

        public string Id { get; set; }

        /// <summary>
        /// set & Get first name
        /// </summary>
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Set & Get last name
        /// </summary>
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// set & get gender
        /// </summary>
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// set & get date of birth
        /// </summary>
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// set & get email
        /// </summary>
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// set & get phone number
        /// </summary>
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// set & get short description
        /// </summary>
        [Display(Name = "Short Description")]
        public string ShortProfile { get; set; }

        /// <summary>
        /// Qualifications
        /// </summary>
        [Display(Name = "Language")]
        public string Language { get; set; }

        /// <summary>
        /// Registration Number
        /// </summary>
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Video Link
        /// </summary>
        public string VideoLink { get; set; }

        public DateTime DateCreated { get; set; }

        public IList<string> SelectedSpeciality { get; set; }
        public IList<SelectListItem> AvailableSpeciality { get; set; }

        // DOB

        [Display(Name = "Day")]
        public int? DateOfBirthDay { get; set; }

        [Display(Name = "Month")]
        public int? DateOfBirthMonth { get; set; }

        [Display(Name = "Year")]
        public int? DateOfBirthYear { get; set; }

        [Display(Name = "It will be shown in user detail page below user short description.")]
        public string ProfessionalStatements { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Fee must be a Numbers only.")]
        [Display(Name = "Consultation Fee")]
        public decimal ConsultationFee { get; set; }

        [Display(Name = "Is Available 24/7")]
        public bool IsAvailability { get; set; }

       

        public DateTime? ParseDateOfBirth()
        {
            if (!DateOfBirthYear.HasValue || !DateOfBirthMonth.HasValue || !DateOfBirthDay.HasValue)
                return null;

            DateTime? dateOfBirth = null;
            try
            {
                dateOfBirth = new DateTime(DateOfBirthYear.Value, DateOfBirthMonth.Value, DateOfBirthDay.Value);
            }
            catch (Exception)
            {
                //todo log exceptions
            }
            return dateOfBirth;
        }
    }
}