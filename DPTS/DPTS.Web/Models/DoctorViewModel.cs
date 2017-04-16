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
            listReviewComments = new List<DoctorUserReviewComments>();
            ReviewComments = new ReviewComments();
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

        // review
        public virtual IList<DoctorUserReviewComments> listReviewComments { get; set; }
        public virtual ReviewComments ReviewComments { get; set; }

        //picture
        //pictures
        public PictureModel AddPictureModel { get; set; }
        public IList<PictureModel> DoctorPictureModels { get; set; }



    }
}