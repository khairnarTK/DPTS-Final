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
          //  Specialities = new List<Speciality>();
        }
      //  public IList<Speciality> Specialities { get; set; }
        public Doctor Doctors { get; set; }
        public Domain.Entities.Address Address { get; set; }
        public double Distance { get; set; }
        public string AddressLine { get; set; }
        public string YearOfExperience { get; set; }
        public string Qualification { get; set; }
        public string ListSpecialities { get; set; }
    }
}