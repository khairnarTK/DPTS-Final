using System.Collections.Generic;
using DPTS.Domain.Entities;

namespace DPTS.Web.Models
{
    public class DoctorViewModel
    {
        public DoctorViewModel()
        {
            doctor = new Doctor();
            Addresses = new List<Address>();
            Specialitys = new List<Speciality>();
            Schedule=new List<Schedule>();
            DoctorPictureModels = new List<PictureModel>();
            AddPictureModel = new PictureModel();
        }
        public string Id { get; set; }

        public virtual Doctor doctor { get; set; }

        public virtual IList<Address> Addresses { get; set; }

        public virtual IList<Speciality> Specialitys { get; set; }

        public virtual IList<Schedule> Schedule { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        public double Distance { get; set; }

        //pictures
        public PictureModel AddPictureModel { get; set; }
        public IList<PictureModel> DoctorPictureModels { get; set; }
    }
}