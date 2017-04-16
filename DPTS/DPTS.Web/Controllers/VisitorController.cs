using System;
using System.Web.Mvc;
using DPTS.Data.Context;
using DPTS.Domain.Core.Appointment;
using DPTS.Web.Models;
using DPTS.Domain.Core.Doctors;
using Microsoft.AspNet.Identity;
using DPTS.EmailSmsNotifications.IServices;
using DPTS.EmailSmsNotifications.ServiceModels;
using System.Linq;

namespace DPTS.Web.Controllers
{
    public class VisitorController : BaseController
    {
        #region Fields

        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _scheduleService;
        private readonly DPTSDbContext _context;
        private ISmsNotificationService _smsService;

        #endregion

        #region Contr

        public VisitorController(IDoctorService doctorService,
            IAppointmentService scheduleService,
            ISmsNotificationService smsService)
        {
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _context = new DPTSDbContext();
            _smsService = smsService;
        }

        #endregion

        #region Methods
        [NonAction]
        private void SendOtp(string phoneNumber, string message)
        {
            var sms = new SmsNotificationModel
            {
                numbers = phoneNumber,
                route = 4,
                //route 4 is for transactional sms
                senderId = "DOCPTS"
            };
            sms.message = message;
            _smsService.SendSms(sms);
        }
        public ActionResult AppointmentList()
        {
            var model = new VisitorViewModel();

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var visitorSchedule = _scheduleService.GetAppointmentScheduleByPatientId(userId);
                model.AppointmentSchedule = visitorSchedule;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CancelAppoinmant(int appoinmentId)
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    var appoinment = _scheduleService.GetAppointmentScheduleById(appoinmentId);
                    if (appoinment == null)
                        return HttpNotFound();

                    var status = _scheduleService.GetAppointmentStatusByName("Cancelled");
                    if (status == null)
                        return HttpNotFound();

                    appoinment.StatusId = status.Id;
                    _scheduleService.UpdateAppointmentSchedule(appoinment);

                    string userId = User.Identity.GetUserId();
                    var patient = _context.AspNetUsers.Where(p => p.Id == userId).FirstOrDefault();
                    var doc = _context.Doctors.Where(p => p.DoctorId == appoinment.DoctorId).FirstOrDefault();

                    //Patient alert
                    if (!string.IsNullOrWhiteSpace(patient.PhoneNumber))
                    {
                        string msg = string.Empty;
                        msg += "CONFIRMED Appoinment ID:" + appoinment.Id;
                        msg += " for " + appoinment.AppointmentDate + " at time " + appoinment.AppointmentTime;
                        msg += " with Dr." + doc.AspNetUser.FirstName + " " + doc.AspNetUser.LastName;
                        msg += " Ph:" + doc.AspNetUser.PhoneNumber;
                        msg += "To Cancel login on Doctor 365";
                        SendOtp(patient.PhoneNumber, msg);
                    }
                    //Doctor Alert
                    if (!string.IsNullOrWhiteSpace(doc.AspNetUser.PhoneNumber))
                    {
                        string msg = string.Empty;
                        msg += "CONFIRMED Appoinment ID:" + appoinment.Id;
                        msg += " for " + appoinment.AppointmentDate + " at time " + appoinment.AppointmentTime;
                        msg += " Patient:" + patient.FirstName + " " + patient.LastName;
                        msg += " Ph:" + patient.PhoneNumber;
                        SendOtp(patient.PhoneNumber, msg);
                    }

                    return Json(new
                    {
                        redirect = Url.Action("AppointmentList", "Visitor"),
                    });
                }
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
            return HttpNotFound();
        }

        #endregion
    }
}