using DPTS.Domain.Entities;
using System.Collections.Generic;

namespace DPTS.Web.Models
{
    public class TempDoctorViewModel
    {
        public TempDoctorViewModel()
        {
            Doctors = new Doctor();
            Address = new Address();
            ReviewOverviewModel = new DoctorReviewOverviewModel();
            AddPictureModel = new PictureModel();
        }

        public Doctor Doctors { get; set; }
        public Domain.Entities.Address Address { get; set; }
        public double Distance { get; set; }
        public string AddressLine { get; set; }
        public string YearOfExperience { get; set; }
        public string Qualification { get; set; }
        public string ListSpecialities { get; set; }
        public PictureModel AddPictureModel { get; set; }
        public DoctorReviewOverviewModel ReviewOverviewModel { get; set; }
    }
}