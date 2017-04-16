using System.Collections.Generic;
using DPTS.Domain.Entities;

namespace DPTS.Web.Models
{
    public class DoctorScheduleListingViewModel
    {
        public DoctorScheduleListingViewModel()
        {
            AppointmentSchedule = new List<AppointmentSchedule>();
        }

        public IList<AppointmentSchedule> AppointmentSchedule { get; set; }

        public string ByDate { get; set; }
    }
}