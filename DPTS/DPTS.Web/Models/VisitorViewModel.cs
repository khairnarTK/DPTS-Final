using System;
using System.Collections.Generic;
using DPTS.Domain.Entities;

namespace DPTS.Web.Models
{
    public class VisitorViewModel
    {
        public VisitorViewModel()
        {
            AppointmentSchedule = new List<AppointmentSchedule>();
            AppointmentStatus = new AppointmentStatus();
        }

        public IList<AppointmentSchedule> AppointmentSchedule { get; set; }


        private AppointmentStatus AppointmentStatus
        {
            set { if (value == null) throw new ArgumentNullException(nameof(value)); }
        }
    }
}