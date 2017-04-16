using DPTS.Domain.Core.Appointment;
using DPTS.Domain.Core.Doctors;
using DPTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DPTS.Data.Context;
using DPTS.Domain.Entities;
using System.Text;
using DPTS.Services;
using DPTS.EmailSmsNotifications.ServiceModels;
using DPTS.EmailSmsNotifications.IServices;

namespace DPTS.Web.Controllers
{
    public class AppointmentController : BaseController
    {
        #region Fields

        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _scheduleService;
        private readonly DPTSDbContext _context;
        private ISmsNotificationService _smsService;


        #endregion

        #region Contr

        public AppointmentController(IDoctorService doctorService,
            IAppointmentService scheduleService,
            ISmsNotificationService smsService)
        {
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _context = new DPTSDbContext();
            _smsService = smsService;
        }

        #endregion

        #region Utilities

        private static AppointmentScheduleViewModel GenrateTimeSlots(string startTime,
            string endTime, double duration,
            IList<AppointmentSchedule> bookedSlots)
        {
            try
            {
                var slots = new AppointmentScheduleViewModel();

                var start = DateTime.Parse(startTime);
                var end = DateTime.Parse(endTime);
                while (true)
                {
                    var dtNext = start.AddMinutes(duration);
                    if (start > end || dtNext > end)
                        break;

                    var slot = start.ToString("hh:mm tt") + " - " + dtNext.ToString("hh:mm tt");
                    var bookingstatus = bookedSlots.Where(s => s.AppointmentTime == slot).FirstOrDefault();

                    var splitSlot = new ScheduleSlotModel
                    {
                        Slot = slot,
                        IsBooked = bookingstatus == null ? false : true
                    };
                    slots.ScheduleSlotModel.Add(splitSlot);
                    start = dtNext;
                }
                return slots;
            }
            catch
            {
                return null;
            }
        }
        [NonAction]
        public AppointmentScheduleViewModel GetAvailableSchedule(string doctorId)
        {
            try
            {
                string todayDay = DateTime.UtcNow.ToString("dddd");
                var schedule =
                    _scheduleService.GetScheduleByDoctorId(doctorId).FirstOrDefault(s => s.Day.Equals(todayDay));
                if (schedule == null)
                    return null;

                var bookedSlots =
                    _scheduleService.GetAppointmentScheduleByDoctorId(doctorId)
                        .Where(s => s.AppointmentDate.Equals(DateTime.UtcNow.ToString("yyyy-MM-dd")))
                        .ToList();
                bookedSlots =
                    bookedSlots.Where(
                        s => s.AppointmentStatus.Name == "Pending" || s.AppointmentStatus.Name == "Booked")
                        .ToList();

                var scheduleSlots = GenrateTimeSlots(schedule.StartTime, schedule.EndTime, 20, bookedSlots);

                return scheduleSlots;
            }
            catch { return null; }
        }

        private void SendOtp(string phoneNumber,string message)
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

        #endregion

        #region Methods

        [HttpGet]
        public ActionResult Booking(string doctorId, string selectedDate = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(doctorId))
                {
                    var scheduleSlots = GetAvailableSchedule(doctorId);
                    scheduleSlots.doctorId = doctorId;
                    return View(scheduleSlots);
                }
            }
            catch
            {
                // ignored
            }
            return RedirectToAction("NoSchedule");
        }

        [HttpGet]
        public ActionResult ReScheduling(string doctorId, int bookingId, string userId, string selectedDate = null)
        {
            var model = new AppointmentScheduleViewModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(doctorId) && bookingId > 0 && !string.IsNullOrWhiteSpace(userId))
                {
                    model = GetAvailableSchedule(doctorId);
                    model.doctorId = doctorId;
                    model.AppointmentSchedule = _scheduleService.GetAppointmentScheduleById(bookingId);
                    return View(model);
                }
            }
            catch
            {
                // ignored
            }
            return View(model);
        }

        public JsonResult BookingScheduleByDate(string slot_date, string doctorId)
        {
            var response = new StringBuilder();
            string resultNoResultFound = string.Empty;

            if (!string.IsNullOrWhiteSpace(doctorId) && !string.IsNullOrWhiteSpace(slot_date))
            {
                resultNoResultFound += "<div class=\"bk-thanks-message\">";
                resultNoResultFound += "<div class=\"tg-message\">";
                resultNoResultFound += "<h2>Schedule Not Found !!</h2>";
                resultNoResultFound += "<div class=\"tg-description\">";
                resultNoResultFound += "<p>Appointment not available...</p>";
                resultNoResultFound += "</div></div></div>";

                string todayDay = DateTime.Parse(slot_date).ToString("dddd");
                //DateTime.UtcNow.ToString("dddd");
                var schedule =
                    _scheduleService.GetScheduleByDoctorId(doctorId).FirstOrDefault(s => s.Day.Equals(todayDay));
                if (schedule == null)
                {
                    return Json(new
                    {
                        result = resultNoResultFound
                    });
                }
                var bookedSlots =
                    _scheduleService.GetAppointmentScheduleByDoctorId(doctorId)
                        .Where(s => s.AppointmentDate.Equals(slot_date))
                        .ToList();

                bookedSlots =
                    bookedSlots.Where(
                        s => s.AppointmentStatus.Name == "Pending" || s.AppointmentStatus.Name == "Booked")
                        .ToList();


                var scheduleSlots = GenrateTimeSlots(schedule.StartTime, schedule.EndTime, 20, bookedSlots);

                if (scheduleSlots == null)
                {
                    return Json(new
                    {
                        result = resultNoResultFound
                    });
                }

                foreach (var item in scheduleSlots.ScheduleSlotModel)
                {
                    string repo = string.Empty;
                    if (item.IsBooked)
                    {
                        repo = "<div class=\"tg-doctimeslot tg-booked\">";
                        repo += "<div class=\"tg-box\">";
                        repo += "<div class=\"tg-radio\">";
                        repo += "<input id = \"" + item.Slot + "\" value=\"" + item.Slot +
                                "\" type=\"radio\" name=\"slottime\" disabled >";
                        repo += "<label for=\"" + item.Slot + "\">" + item.Slot + "</label>";
                        repo += "</div> </div> </div>";
                    }
                    else
                    {
                        repo = "<div class=\"tg-doctimeslot tg-available\">";
                        repo += "<div class=\"tg-box\">";
                        repo += "<div class=\"tg-radio\">";
                        repo += "<input id = \"" + item.Slot + "\" value=\"" + item.Slot +
                                "\" type=\"radio\" name=\"slottime\">";
                        repo += "<label for=\"" + item.Slot + "\">" + item.Slot + "</label>";
                        repo += "</div> </div> </div>";
                    }
                    response.AppendLine(repo);
                }
                return Json(new
                {
                    response = response.ToString()
                });
            }
            return Json(new
            {
                success = 1
            });
        }

        [HttpPost]
        public ActionResult AvailableSchedule(AppointmentScheduleViewModel model, string Command)
        {
            if (Command == "next" && !string.IsNullOrWhiteSpace(model.doctorId))
            {
                return RedirectToAction("VisitorContactDeatils", new {doctorId = model.doctorId});
            }
            return View();
        }

        public ActionResult NoSchedule()
        {
            return View();
        }

        public JsonResult VisitorContactDeatils()
        {
            if (!Request.IsAuthenticated)
                return Json(new
                {
                    success = 1
                });

            var userId = User.Identity.GetUserId();
            var visitor = _context.AspNetUsers.SingleOrDefault(u => u.Id == userId);
            if (visitor == null)
            {
                return Json(new
                {
                    success = 1
                });
            }
            var fullName = visitor.FirstName + " " + visitor.LastName;
            return Json(new
            {
                mobilenumber = visitor.PhoneNumber,
                useremail = visitor.Email,
                username = fullName
            });
        }

        public JsonResult VerifyReScheduling(string doctorId,string patientId,int bookingId)
        {
            if (!Request.IsAuthenticated)
                return new NullJsonResult();

            if (string.IsNullOrEmpty(doctorId) || !User.IsInRole("Doctor") || bookingId < 0)
                return new NullJsonResult();

            var appoinment = _scheduleService.GetAppointmentScheduleById(bookingId);
            if(appoinment.DoctorId.Equals(doctorId) 
                && appoinment.PatientId.Equals(patientId)
                && appoinment.Id.Equals(bookingId)
                && !doctorId.Equals(patientId))
            {
                return Json(new
                {
                    result = "success"
                });
            }

            return new NullJsonResult();
        }

        public JsonResult PaymentMode()
        {
            return Json(new
            {
                success = 1
            });
        }

        public JsonResult FinishBooking(FormCollection form)
        {
            if (!string.IsNullOrWhiteSpace(form["booking_date"]) &&
                !string.IsNullOrWhiteSpace("slottime") &&
                !string.IsNullOrWhiteSpace("subject") &&
                !string.IsNullOrWhiteSpace("username") &&
                !string.IsNullOrWhiteSpace("mobilenumber") &&
                !string.IsNullOrWhiteSpace("useremail") &&
                !string.IsNullOrWhiteSpace("booking_note") &&
                !string.IsNullOrWhiteSpace("payment") &&
                !string.IsNullOrWhiteSpace("doctorId") &&
                Request.IsAuthenticated)
            {
                const string statusFlag = "Pending";
                var bookingStatus = _scheduleService.GetAppointmentStatusByName(statusFlag);
                var userId = User.Identity.GetUserId();

                var booking = new AppointmentSchedule
                {
                    DoctorId = form["doctorId"],
                    PatientId = userId,
                    Subject = form["subject"],
                    DiseasesDescription = form["booking_note"],
                    AppointmentTime = form["slottime"].Trim(),
                    StatusId = bookingStatus.Id,
                    AppointmentDate = form["booking_date"]
                };

                _scheduleService.InsertAppointmentSchedule(booking);
                var patient = _context.AspNetUsers.Where(p => p.Id == userId).FirstOrDefault();
                var doc = _context.Doctors.Where(p => p.DoctorId == booking.DoctorId).FirstOrDefault();
                //Patient alert
                if (!string.IsNullOrWhiteSpace(patient.PhoneNumber))
                {
                    string msg = string.Empty;
                    msg += "CONFIRMED Appoinment ID:" + booking.Id;
                    msg += " for " + booking.AppointmentDate + " at time " + booking.AppointmentTime;
                    msg += " with Dr." + doc.AspNetUser.FirstName + " " + doc.AspNetUser.LastName;
                    msg += " Ph:" + doc.AspNetUser.PhoneNumber;
                    msg += "To Cancel login on Doctor 365";
                    SendOtp(patient.PhoneNumber, msg);
                }
                //Doctor Alert
                if(!string.IsNullOrWhiteSpace(doc.AspNetUser.PhoneNumber))
                {
                    string msg = string.Empty;
                    msg += "CONFIRMED Appoinment ID:" + booking.Id;
                    msg += " for " + booking.AppointmentDate + " at time " + booking.AppointmentTime;
                    msg += " Patient:" + patient.FirstName + " " + patient.LastName;
                    msg += " Ph:" + patient.PhoneNumber;
                    SendOtp(patient.PhoneNumber, msg);
                }
                return Json(new
                {
                    result = "success"
                });
            }
            return Json(new
            {
                result = "fail"
            });
        }

        public JsonResult FinishReScheduling(FormCollection form)
        {
            if (!string.IsNullOrWhiteSpace(form["booking_date"]) &&
                !string.IsNullOrWhiteSpace(form["slottime"]) &&
                !string.IsNullOrWhiteSpace(form["DoctorId"]) &&
                !string.IsNullOrWhiteSpace(form["PatientId"]) &&
                !string.IsNullOrWhiteSpace(form["BookingId"]) &&
                Request.IsAuthenticated &&
                User.IsInRole("Doctor"))
              {
                const string statusFlag = "Booked";
                var bookingStatus = _scheduleService.GetAppointmentStatusByName(statusFlag);

                var appointment = _scheduleService.GetAppointmentScheduleById(int.Parse(form["BookingId"]));
                if (appointment == null)
                    return new NullJsonResult();

                appointment.AppointmentDate = form["booking_date"];
                appointment.AppointmentTime = form["slottime"].Trim();
                appointment.StatusId = bookingStatus.Id;
                _scheduleService.UpdateAppointmentSchedule(appointment);
                
                return Json(new
                {
                    result = "success"
                });
            }
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult Finish(string Command)
        {
            return View();
        }

        public JsonResult OpcSaveShipping()
        {
            return Json(new
            {
                action_type = "cancelled"
            });
        }

        #endregion
    }
}