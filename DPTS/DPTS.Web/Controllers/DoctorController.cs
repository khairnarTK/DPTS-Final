using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using DPTS.Domain.Core.Address;
using DPTS.Domain.Core.Appointment;
using DPTS.Domain.Core.Country;
using DPTS.Domain.Core.Doctors;
using DPTS.Domain.Core.Speciality;
using DPTS.Domain.Core.StateProvince;
using DPTS.Domain.Entities;
using DPTS.Web.Models;
using Microsoft.AspNet.Identity;
using DPTS.Domain.Core.ReviewComments;
using DPTS.Common.Kendoui;
using HttpVerbs = System.Web.Mvc.HttpVerbs;
using DPTS.Services;
using DPTS.Domain.Common;
using DPTS.EmailSmsNotifications.IServices;
using DPTS.EmailSmsNotifications.ServiceModels;
using DPTS.Data.Context;
using System.Threading.Tasks;

namespace DPTS.Web.Controllers
{
    public class DoctorController : BaseController
    {
        #region Fields

        private readonly IDoctorService _doctorService;
        private readonly ISpecialityService _specialityService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IAddressService _addressService;
        private readonly IAppointmentService _scheduleService;
        private readonly ApplicationDbContext _context;
        private readonly IReviewCommentsService _reviewCommentsService;
        private readonly IPictureService _pictureService;
        private ISmsNotificationService _smsService;
        private readonly DPTSDbContext context;
        #endregion

        #region Contructor
        public DoctorController(IDoctorService doctorService, ISpecialityService specialityService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IAddressService addressService, IAppointmentService scheduleService,
            IReviewCommentsService reviewCommentsService,
            IPictureService pictureService,
            ISmsNotificationService smsService)
        {
            _doctorService = doctorService;
            _context = new ApplicationDbContext();
            _specialityService = specialityService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _addressService = addressService;
            _scheduleService = scheduleService;
            _reviewCommentsService = reviewCommentsService;
            _pictureService = pictureService;
            _smsService = smsService;
            context = new DPTSDbContext();
        }

        #endregion

        #region Utilities

        [NonAction]
        public ApplicationUser GetUserById(string userId)
        {
            return _context.Users.SingleOrDefault(u => u.Id == userId);
        }

        [NonAction]
        private static List<SelectListItem> GetGender()
        {
            List<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem {Text = "Select Gender", Value = "0"}
            };
            items.AddRange(from object gender in Enum.GetValues(typeof(Gender))
                           select new SelectListItem
                           {
                               Text = Enum.GetName(typeof(Gender), gender),
                               Value = Enum.GetName(typeof(Gender), gender)
                           });
            return items;
        }

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

        public IList<SelectListItem> GetAllSpecialities(string docterId)
        {
            var models = new List<SelectListItem>();
            var data = _specialityService.GetAllSpeciality(false);

            foreach (var speciality in data)
            {
                var specilityMap = new SpecialityMapping
                {
                    Speciality_Id = speciality.Id,
                    Doctor_Id = docterId
                };
                if (_specialityService.IsDoctorSpecialityExists(specilityMap))
                {
                    models.Add(new SelectListItem
                    {
                        Text = speciality.Title,
                        Value = speciality.Id.ToString(),
                        Selected = true
                    });
                }
                else
                {
                    models.Add(new SelectListItem
                    {
                        Text = speciality.Title,
                        Value = speciality.Id.ToString()
                    });
                }
            }

            return models;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetStatesByCountryId(int countryId, bool addSelectStateItem)
        {
            //this action method gets called via an ajax request
            if (countryId == 0)
                throw new ArgumentNullException("countryId");

            var country = _countryService.GetCountryById(countryId);
            var states = _stateProvinceService.GetStateProvincesByCountryId(country?.Id ?? 0).ToList();
            var result = (from s in states
                          select new { id = s.Id, name = s.Name })
                .ToList();


            if (country == null)
            {
                //country is not selected ("choose country" item)
                result.Insert(0, addSelectStateItem ? new { id = 0, name = "select state" } : new { id = 0, name = "None" });
            }
            else
            {
                //some country is selected
                if (!result.Any())
                {
                    //country does not have states
                    result.Insert(0, new { id = 0, name = "None" });
                }
                else
                {
                    //country has some states
                    if (addSelectStateItem)
                    {
                        result.Insert(0, new { id = 0, name = "select state" });
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public IList<SelectListItem> GetCountryList()
        {
            var countries = _countryService.GetAllCountries(true);
            var typelst = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                }
            };
            typelst.AddRange(countries.ToList().Select(type => new SelectListItem
            {
                Text = type.Name,
                Value = type.Id.ToString()
            }));
            return typelst;
        }


        [NonAction]
        private static Dictionary<string, double> GetGeoCoordinate(string address)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            try
            {
                string requestUri = $"http://maps.google.com/maps/api/geocode/xml?address={address}&sensor=false";
                var request = System.Net.WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());
                var xElement = xdoc.Element(AppInfra.Constants.GeocodeResponse);
                var result = xElement?.Element(AppInfra.Constants.Result);
                var locationElement = result?.Element(AppInfra.Constants.Geometry)?.Element(AppInfra.Constants.Location);
                ParseLatLong(dictionary, locationElement);
            }
            catch (Exception ex)
            {
            }
            return dictionary;
        }

        private static void ParseLatLong(Dictionary<string, double> dictionary, XElement locationElement)
        {
            if (locationElement != null)
            {
                var lat = locationElement.Element(AppInfra.Constants.Lat);
                if (lat != null)
                    dictionary.Add(AppInfra.Constants.Lat, Double.Parse(lat.Value));
                var _long = locationElement.Element(AppInfra.Constants.Lng);
                if (_long != null)
                    dictionary.Add(AppInfra.Constants.Lng, Double.Parse(_long.Value));
            }
        }

        private static void GetDoctorScheduleByDay(SheduleViewModel obj, IList<Schedule> schedule, string day)
        {
            var scheduleSunday = schedule?.FirstOrDefault(s => s.Day.Equals(day));
            if (scheduleSunday == null || !scheduleSunday.Day.Equals(day))
                obj.Day = day;
            else
            {
                obj.DoctorId = scheduleSunday.DoctorId;
                obj.Day = scheduleSunday.Day;
                obj.EndTime = scheduleSunday.EndTime;
                obj.StartTime = scheduleSunday.StartTime;
            }
        }

        private void GetScheduleByDay(FormCollection form, string day)
        {
            if (!string.IsNullOrWhiteSpace(day))
            {
                var lowerDay = day.ToLower();
                var lowerDayStart = day.ToLower() + "_start";
                var lowerDayEnd = day.ToLower() + "_end";
                if (!string.IsNullOrWhiteSpace(form[lowerDayStart]) &&
                    !string.IsNullOrWhiteSpace(form[lowerDayEnd]))
                {
                    var schedule =
                        _scheduleService
                            .GetScheduleByDoctorId(User.Identity.GetUserId())
                            .FirstOrDefault(s => s.Day.Equals(day));

                    if (schedule == null)
                    {
                        var model = new Schedule
                        {
                            Day = lowerDay,
                            DoctorId = User.Identity.GetUserId(),
                            StartTime = form[lowerDayStart].Trim(),
                            EndTime = form[lowerDayEnd].Trim()
                        };
                        _scheduleService.InsertSchedule(model);
                    }
                    else
                    {
                        schedule.StartTime = form[lowerDayStart].Trim();
                        schedule.EndTime = form[lowerDayEnd].Trim();
                        _scheduleService.UpdateSchedule(schedule);
                    }
                }
            }
        }

        #endregion

        #region Methods

        #region Profile Settings
        public ActionResult ProfileSetting()
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var model = new DoctorProfileSettingViewModel();
            if (Request.IsAuthenticated && User.IsInRole("Doctor"))
            {
                var userId = User.Identity.GetUserId();
                var user = _context.Users.SingleOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    var doctor = _doctorService.GetDoctorbyId(user.Id);
                    if (doctor != null)
                    {
                        model.DateCreated = doctor.DateCreated;
                        var dateOfBirth = string.IsNullOrWhiteSpace(doctor.DateOfBirth)
                            ? (DateTime?)null
                            : DateTime.Parse(doctor.DateOfBirth);
                        if (dateOfBirth.HasValue)
                        {
                            model.DateOfBirthDay = dateOfBirth.Value.Day;
                            model.DateOfBirthMonth = dateOfBirth.Value.Month;
                            model.DateOfBirthYear = dateOfBirth.Value.Year;
                        }
                        model.Gender = doctor.Gender;
                        model.ShortProfile = doctor.ShortProfile;
                        model.Language = doctor.Language;
                        model.RegistrationNumber = doctor.RegistrationNumber;
                        model.ProfessionalStatements = doctor.ProfessionalStatements;
                        model.VideoLink = doctor.VideoLink;
                        model.ConsultationFee = doctor.ConsultationFee;
                        model.IsAvailability = doctor.IsAvailability;
                    }
                }
                if (user != null)
                {
                    model.Email = user.Email;
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.Id = user.Id;
                    model.PhoneNumber = user.PhoneNumber;
                    model.AvailableSpeciality = GetAllSpecialities(user.Id);
                }
            }
            ViewBag.GenderList = GetGender();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProfileSetting(DoctorProfileSettingViewModel model)
        {
            try
            {
                if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                    return new HttpUnauthorizedResult();

                // var doctorSpec = GetAllSpecialities(model.id).Select(c => c.Selected == true);
                if (model.SelectedSpeciality.Count > 0)
                {
                    var specilities = string.Join(",", model.SelectedSpeciality);
                    foreach (var specilityMap in specilities.Split(',').ToList().Select(item => new SpecialityMapping
                    {
                        Speciality_Id = int.Parse(item),
                        Doctor_Id = model.Id,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    }).Where(specilityMap => !_specialityService.IsDoctorSpecialityExists(specilityMap)))
                    {
                        _specialityService.AddSpecialityByDoctor(specilityMap);
                    }
                }

                var doctor = _doctorService.GetDoctorbyId(model.Id);
                if (doctor == null)
                    return null;

                doctor.Gender = model.Gender;
                doctor.ShortProfile = model.ShortProfile;
                var dateOfBirth = model.ParseDateOfBirth();
                doctor.DateOfBirth = dateOfBirth.ToString();
                doctor.DateUpdated = DateTime.UtcNow;
                doctor.Language = model.Language;
                doctor.RegistrationNumber = model.RegistrationNumber;
                doctor.ProfessionalStatements = model.ProfessionalStatements;
                doctor.VideoLink = model.VideoLink;
                doctor.AspNetUser.LastName = model.LastName;
                doctor.AspNetUser.FirstName = model.FirstName;
                doctor.AspNetUser.Email = model.Email;
                doctor.AspNetUser.PhoneNumber = model.PhoneNumber;
                doctor.IsAvailability = model.IsAvailability;
                doctor.ConsultationFee = model.ConsultationFee;
                _doctorService.UpdateDoctor(doctor);
                SuccessNotification("Profile updated successfully.");
                return RedirectToAction("ProfileSetting");
            }
            catch(Exception ex)
            {
                ErrorNotification("Profile updated successfully.");
                return RedirectToAction("ProfileSetting");
            }
        }

        #endregion

        #region Address

        public ActionResult AddressAdd()
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var model = new AddressViewModel { AvailableCountry = GetCountryList() };
            return View(model);
        }


        [HttpPost]
        public ActionResult AddressAdd(AddressViewModel model)
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            if(model.CountryId == 0 || model.StateProvinceId == 0)
            {
                ModelState.AddModelError("Select Country or State !!","");
                ErrorNotification("Select Country or State");
            }

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = _context.Users.SingleOrDefault(u => u.Id == userId);

                var address = new Address
                {
                    StateProvinceId = model.StateProvinceId,
                    CountryId = model.CountryId,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Hospital = model.Hospital,
                    FaxNumber = model.FaxNumber,
                    PhoneNumber = model.LandlineNumber,
                    Website = model.Website,
                    ZipPostalCode = model.ZipPostalCode,
                    City = model.City
                };
                if (address.CountryId == 0)
                    address.CountryId = null;
                if (address.StateProvinceId == 0)
                    address.StateProvinceId = null;

                string state = address.StateProvinceId == 0
                    ? string.Empty
                    : _stateProvinceService.GetStateProvinceById(model.StateProvinceId).Name;
                string docAddress = model.Address1 + ", " + model.City + ", " + state + ", " + model.ZipPostalCode;
                var geoCoodrinate = GetGeoCoordinate(docAddress);
                if (geoCoodrinate.Count == 2)
                {
                    address.Latitude = geoCoodrinate[AppInfra.Constants.Lat];
                    address.Longitude = geoCoodrinate[AppInfra.Constants.Lng];
                }
                else
                {
                    var geoCoodrinates = GetGeoCoordinate(model.ZipPostalCode);
                    if (geoCoodrinates.Count == 2)
                    {
                        address.Latitude = geoCoodrinates[AppInfra.Constants.Lat];
                        address.Longitude = geoCoodrinates[AppInfra.Constants.Lng];
                    }
                }
                _addressService.AddAddress(address);
                if (user != null)
                {
                    var addrMap = new AddressMapping
                    {
                        AddressId = address.Id,
                        UserId = user.Id
                    };
                    _addressService.AddAddressMapping(addrMap);
                }
                SuccessNotification("Address added successfully.");

                return RedirectToAction("Addresses");
            }
            model.AvailableCountry = GetCountryList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddressDelete(int addressId)
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
                return Json(new
                {
                    redirect = Url.Action("Addresses", "Doctor"),
                });

            var addrMapping = _addressService.GetAddressMappingbuUserIdAddrId(user.Id, addressId);

            if (addrMapping == null)
                return Content("No Customer found with the specified id");

            _addressService.DeleteAddressMapping(addrMapping);
            var address = _addressService.GetAddressbyId(addressId);
            if (address == null)
                return Json(new
                {
                    redirect = Url.Action("Addresses", "Doctor"),
                });
            _addressService.DeleteAddress(address);


            return Json(new
            {
                redirect = Url.Action("Addresses", "Doctor"),
            });
        }

        public ActionResult Addresses()
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            var model = new List<AddressViewModel>();

            if (user != null)
                model.AddRange(_addressService.GetAllAddressByUser(user.Id).Select(c => new AddressViewModel
                {
                    Id = c.Id,
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    City = c.City,
                    CountryName =
                        c.CountryId.GetValueOrDefault() > 0
                            ? _countryService.GetCountryById(c.CountryId.GetValueOrDefault()).Name
                            : " ",
                    StateName =
                        c.StateProvinceId.GetValueOrDefault() > 0
                            ? _stateProvinceService.GetStateProvinceById(c.StateProvinceId.GetValueOrDefault()).Name
                            : " ",
                    FaxNumber = c.FaxNumber,
                    Hospital = c.Hospital,
                    LandlineNumber = c.PhoneNumber,
                    Website = c.Website,
                    ZipPostalCode = c.ZipPostalCode,
                }).ToList());

            return View(model);
        }

        public ActionResult AddressEdit(int id)
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var address = _addressService.GetAddressbyId(id);
            if (address == null)
                return null;

            var model = new AddressViewModel
            {
                Id = address.Id,
                Address1 = address.Address1,
                Address2 = address.Address2,
                City = address.City,
                CountryId = address.CountryId.GetValueOrDefault(),
                FaxNumber = address.FaxNumber,
                Hospital = address.Hospital,
                LandlineNumber = address.PhoneNumber,
                StateProvinceId = address.StateProvinceId.GetValueOrDefault(),
                Website = address.Website,
                ZipPostalCode = address.ZipPostalCode,
                AvailableCountry = GetCountryList(),
                //AvailableStateProvince= GetStatesByCountryId(address.CountryId,true)
            };

            model.AvailableCountry = GetCountryList();

            //states
            var states = _stateProvinceService
                .GetStateProvincesByCountryId(model.CountryId)
                .ToList();
            if (states.Any())
            {
                model.AvailableStateProvince.Add(new SelectListItem { Text = "Select state", Value = "0" });

                foreach (var s in states)
                {
                    model.AvailableStateProvince.Add(new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                        Selected = s.Id == model.StateProvinceId
                    });
                }
            }
            else
            {
                bool anyCountrySelected = model.AvailableCountry.Any(x => x.Selected);
                model.AvailableStateProvince.Add(new SelectListItem
                {
                    Text = anyCountrySelected ? "None" : "Select State",
                    Value = "0"
                });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddressEdit(AddressViewModel model)
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                var address = _addressService.GetAddressbyId(model.Id);
                address.Id = model.Id;
                address.Address1 = model.Address1;
                address.Address2 = model.Address2;
                address.City = model.City;
                address.CountryId = model.CountryId;
                address.FaxNumber = model.FaxNumber;
                address.Hospital = model.Hospital;
                address.PhoneNumber = model.LandlineNumber;
                address.StateProvinceId = model.StateProvinceId;
                address.Website = model.Website;
                address.ZipPostalCode = model.ZipPostalCode;

                string state = address.StateProvinceId == 0
                    ? string.Empty
                    : _stateProvinceService.GetStateProvinceById(model.StateProvinceId).Name;
                string docAddress = model.Address1 + ", " + model.City + ", " + state + ", " + model.ZipPostalCode;
                var geoCoodrinate = GetGeoCoordinate(docAddress);
                if (geoCoodrinate.Count == 2)
                {
                    address.Latitude = geoCoodrinate["lat"];
                    address.Longitude = geoCoodrinate["lng"];
                }
                else
                {
                    var geoCoodrinates = GetGeoCoordinate(model.ZipPostalCode);
                    if (geoCoodrinates.Count == 2)
                    {
                        address.Latitude = geoCoodrinates["lat"];
                        address.Longitude = geoCoodrinates["lng"];
                    }
                }

                _addressService.UpdateAddress(address);
                return RedirectToAction("Addresses");
            }
            //states

            model.AvailableCountry = GetCountryList();

            var states = _stateProvinceService
                .GetStateProvincesByCountryId(model.CountryId)
                .ToList();
            if (states.Any())
            {
                model.AvailableStateProvince.Add(new SelectListItem { Text = "Select state", Value = "0" });

                foreach (var s in states)
                {
                    model.AvailableStateProvince.Add(new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                        Selected = s.Id == model.StateProvinceId
                    });
                }
            }
            else
            {
                bool anyCountrySelected = model.AvailableCountry.Any(x => x.Selected);
                model.AvailableStateProvince.Add(new SelectListItem
                {
                    Text = anyCountrySelected ? "None" : "Select State",
                    Value = "0"
                });
            }
            return View(model);
        }
        #endregion


        #endregion

        #region Favourites & Invoices

        public ActionResult Favourites()
        {
            return View();
        }

        public ActionResult InvoicesPackages()
        {
            return View();
        }

        #endregion

        #region Schedule

        public ActionResult DoctorSchedules()
        {
            try
            {
                var lst = new List<SheduleViewModel>();
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    var obj = new SheduleViewModel();
                    var schedule = _scheduleService.GetScheduleByDoctorId(User.Identity.GetUserId());
                    switch (day)
                    {
                        case DayOfWeek.Sunday:
                            var scheduleSunday = schedule.Where(s => s.Day.Equals("Sunday")).FirstOrDefault();
                            if (scheduleSunday == null || !scheduleSunday.Day.Equals("Sunday"))
                                obj.Day = "Sunday";
                            else
                            {
                                obj.DoctorId = scheduleSunday.DoctorId;
                                obj.Day = scheduleSunday.Day;
                                obj.EndTime = scheduleSunday.EndTime.ToString();
                                obj.StartTime = scheduleSunday.StartTime.ToString();
                            }
                            break;
                        case DayOfWeek.Monday:
                            var scheduleMonday = schedule.Where(s => s.Day.Equals("Monday")).FirstOrDefault();
                            if (scheduleMonday == null || !scheduleMonday.Day.Equals("Monday"))
                                obj.Day = "Monday";
                            else
                            {
                                obj.DoctorId = scheduleMonday.DoctorId;
                                obj.Day = scheduleMonday.Day;
                                obj.EndTime = scheduleMonday.EndTime.ToString();
                                obj.StartTime = scheduleMonday.StartTime.ToString();
                            }
                            break;
                        case DayOfWeek.Tuesday:
                            var scheduleTuesday = schedule.Where(s => s.Day.Equals("Tuesday")).FirstOrDefault();
                            if (scheduleTuesday == null || !scheduleTuesday.Day.Equals("Tuesday"))
                                obj.Day = "Tuesday";
                            else
                            {
                                obj.DoctorId = scheduleTuesday.DoctorId;
                                obj.Day = scheduleTuesday.Day;
                                obj.EndTime = scheduleTuesday.EndTime.ToString();
                                obj.StartTime = scheduleTuesday.StartTime.ToString();
                            }
                            break;
                        case DayOfWeek.Wednesday:
                            var scheduleWednesday = schedule.Where(s => s.Day.Equals("Wednesday")).FirstOrDefault();
                            if (scheduleWednesday == null || !scheduleWednesday.Day.Equals("Wednesday"))
                                obj.Day = "Wednesday";
                            else
                            {
                                obj.DoctorId = scheduleWednesday.DoctorId;
                                obj.Day = scheduleWednesday.Day;
                                obj.EndTime = scheduleWednesday.EndTime.ToString();
                                obj.StartTime = scheduleWednesday.StartTime.ToString();
                            }
                            break;
                        case DayOfWeek.Thursday:
                            var scheduleThursday = schedule.Where(s => s.Day.Equals("Thursday")).FirstOrDefault();
                            if (scheduleThursday == null || !scheduleThursday.Day.Equals("Thursday"))
                                obj.Day = "Thursday";
                            else
                            {
                                obj.DoctorId = scheduleThursday.DoctorId;
                                obj.Day = scheduleThursday.Day;
                                obj.EndTime = scheduleThursday.EndTime.ToString();
                                obj.StartTime = scheduleThursday.StartTime.ToString();
                            }
                            break;
                        case DayOfWeek.Friday:
                            var scheduleFriday = schedule.Where(s => s.Day.Equals("Friday")).FirstOrDefault();
                            if (scheduleFriday == null || !scheduleFriday.Day.Equals("Friday"))
                                obj.Day = "Friday";
                            else
                            {
                                obj.DoctorId = scheduleFriday.DoctorId;
                                obj.Day = scheduleFriday.Day;
                                obj.EndTime = scheduleFriday.EndTime.ToString();
                                obj.StartTime = scheduleFriday.StartTime.ToString();
                            }
                            break;
                        case DayOfWeek.Saturday:
                            var scheduleSaturday = schedule.Where(s => s.Day.Equals("Saturday")).FirstOrDefault();
                            if (scheduleSaturday == null || !scheduleSaturday.Day.Equals("Saturday"))
                                obj.Day = "Saturday";
                            else
                            {
                                obj.DoctorId = scheduleSaturday.DoctorId;
                                obj.Day = scheduleSaturday.Day;
                                obj.EndTime = scheduleSaturday.EndTime.ToString();
                                obj.StartTime = scheduleSaturday.StartTime.ToString();
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    lst.Add(obj);
                }

                return View(lst);
            }
            catch { return null; }
        }

        [HttpPost]
        public ActionResult DoctorSchedules(FormCollection form)
        {
            try
            {
                if (!Request.IsAuthenticated)
                    return HttpNotFound();

                #region Sunday
                if (!string.IsNullOrWhiteSpace(form["sunday_start"].ToString()) &&
                    !string.IsNullOrWhiteSpace(form["sunday_end"].ToString()))
                {
                    var schedule = _scheduleService.GetScheduleByDoctorId(User.Identity.GetUserId()).Where(s => s.Day.Equals("Sunday")).FirstOrDefault();

                    if (schedule == null)
                    {
                        //insert record
                        var model = new Schedule();
                        model.Day = "Sunday";
                        model.DoctorId = User.Identity.GetUserId();
                        model.StartTime = form["sunday_start"].Trim().ToString();
                        model.EndTime = form["sunday_end"].Trim().ToString();
                        _scheduleService.InsertSchedule(model);
                    }
                    else
                    {
                        //update record
                        schedule.StartTime = form["sunday_start"].Trim().ToString();
                        schedule.EndTime = form["sunday_end"].Trim().ToString();
                        _scheduleService.UpdateSchedule(schedule);
                    }
                }
                #endregion

                #region Monday

                if (!string.IsNullOrWhiteSpace(form["monday_start"].ToString()) &&
                   !string.IsNullOrWhiteSpace(form["monday_end"].ToString()))
                {
                    var schedule = _scheduleService.GetScheduleByDoctorId(User.Identity.GetUserId()).Where(s => s.Day.Equals("Monday")).FirstOrDefault();

                    if (schedule == null)
                    {
                        //insert record
                        var model = new Schedule();
                        model.Day = "Monday";
                        model.DoctorId = User.Identity.GetUserId();
                        model.StartTime = form["monday_start"].Trim().ToString();
                        model.EndTime = form["monday_end"].Trim().ToString();
                        _scheduleService.InsertSchedule(model);
                    }
                    else
                    {
                        //update record
                        schedule.StartTime = form["monday_start"].Trim().ToString();
                        schedule.EndTime = form["monday_end"].Trim().ToString();
                        _scheduleService.UpdateSchedule(schedule);
                    }
                }
                #endregion

                #region Tuesday

                if (!string.IsNullOrWhiteSpace(form["tuesday_start"].ToString()) &&
                    !string.IsNullOrWhiteSpace(form["tuesday_end"].ToString()))
                {
                    var schedule = _scheduleService.GetScheduleByDoctorId(User.Identity.GetUserId()).Where(s => s.Day.Equals("Tuesday")).FirstOrDefault();

                    if (schedule == null)
                    {
                        //insert record
                        var model = new Schedule();
                        model.Day = "Tuesday";
                        model.DoctorId = User.Identity.GetUserId();
                        model.StartTime = form["tuesday_start"].Trim().ToString();
                        model.EndTime = form["tuesday_end"].Trim().ToString();
                        _scheduleService.InsertSchedule(model);
                    }
                    else
                    {
                        //update record
                        schedule.StartTime = form["tuesday_start"].Trim().ToString();
                        schedule.EndTime = form["tuesday_end"].Trim().ToString();
                        _scheduleService.UpdateSchedule(schedule);
                    }
                }
                #endregion

                #region Wednesday

                //Wednesday
                if (!string.IsNullOrWhiteSpace(form["wednesday_start"].ToString()) &&
                   !string.IsNullOrWhiteSpace(form["wednesday_end"].ToString()))
                {
                    var schedule = _scheduleService.GetScheduleByDoctorId(User.Identity.GetUserId())
                        .Where(s => s.Day.Equals("Wednesday")).FirstOrDefault();

                    if (schedule == null)
                    {
                        //insert record
                        var model = new Schedule();
                        model.Day = "Wednesday";
                        model.DoctorId = User.Identity.GetUserId();
                        model.StartTime = form["wednesday_start"].Trim().ToString();
                        model.EndTime = form["wednesday_end"].Trim().ToString();
                        _scheduleService.InsertSchedule(model);
                    }
                    else
                    {
                        //update record
                        schedule.StartTime = form["wednesday_start"].Trim().ToString();
                        schedule.EndTime = form["wednesday_end"].Trim().ToString();
                        _scheduleService.UpdateSchedule(schedule);
                    }
                }
                #endregion

                #region Thursday
                //Thursday
                if (!string.IsNullOrWhiteSpace(form["thursday_start"].ToString()) &&
                  !string.IsNullOrWhiteSpace(form["thursday_end"].ToString()))
                {
                    var schedule = _scheduleService.GetScheduleByDoctorId(User.Identity.GetUserId())
                        .Where(s => s.Day.Equals("Thursday")).FirstOrDefault();

                    if (schedule == null)
                    {
                        //insert record
                        var model = new Schedule();
                        model.Day = "Thursday";
                        model.DoctorId = User.Identity.GetUserId();
                        model.StartTime = form["thursday_start"].Trim().ToString();
                        model.EndTime = form["thursday_end"].Trim().ToString();
                        _scheduleService.InsertSchedule(model);
                    }
                    else
                    {
                        //update record
                        schedule.StartTime = form["thursday_start"].Trim().ToString();
                        schedule.EndTime = form["thursday_end"].Trim().ToString();
                        _scheduleService.UpdateSchedule(schedule);
                    }
                }
                #endregion

                #region Friday
                //Friday
                if (!string.IsNullOrWhiteSpace(form["friday_start"].ToString()) &&
                 !string.IsNullOrWhiteSpace(form["friday_end"].ToString()))
                {
                    var schedule = _scheduleService.GetScheduleByDoctorId(User.Identity.GetUserId())
                        .Where(s => s.Day.Equals("Friday")).FirstOrDefault();

                    if (schedule == null)
                    {
                        //insert record
                        var model = new Schedule();
                        model.Day = "Friday";
                        model.DoctorId = User.Identity.GetUserId();
                        model.StartTime = form["friday_start"].Trim().ToString();
                        model.EndTime = form["friday_end"].Trim().ToString();
                        _scheduleService.InsertSchedule(model);
                    }
                    else
                    {
                        //update record
                        schedule.StartTime = form["friday_start"].Trim().ToString();
                        schedule.EndTime = form["friday_end"].Trim().ToString();
                        _scheduleService.UpdateSchedule(schedule);
                    }
                }
                #endregion

                #region Saturday
                //Saturday
                if (!string.IsNullOrWhiteSpace(form["saturday_start"].ToString()) &&
                !string.IsNullOrWhiteSpace(form["saturday_end"].ToString()))
                {
                    var schedule = _scheduleService.GetScheduleByDoctorId(User.Identity.GetUserId())
                        .Where(s => s.Day.Equals("Saturday")).FirstOrDefault();

                    if (schedule == null)
                    {
                        //insert record
                        var model = new Schedule();
                        model.Day = "Saturday";
                        model.DoctorId = User.Identity.GetUserId();
                        model.StartTime = form["saturday_start"].Trim().ToString();
                        model.EndTime = form["saturday_end"].Trim().ToString();
                        _scheduleService.InsertSchedule(model);
                    }
                    else
                    {
                        //update record
                        schedule.StartTime = form["saturday_start"].Trim().ToString();
                        schedule.EndTime = form["saturday_end"].Trim().ToString();
                        _scheduleService.UpdateSchedule(schedule);
                    }
                }
                #endregion
                SuccessNotification("Schedules updated successfully.");
                return RedirectToAction("DoctorSchedules");
            }
            catch { return null; }
        }

        #endregion

        #region Booking

        public ActionResult BookingListings(DoctorScheduleListingViewModel model)
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            try
            {
                if (Request.IsAuthenticated && User.IsInRole("Doctor"))
                {
                    var userId = User.Identity.GetUserId();
                    var doctorSchedule = _scheduleService.GetAppointmentScheduleByDoctorId(userId);

                    if (!string.IsNullOrWhiteSpace(model.ByDate))
                    {
                        var sortedByDate = doctorSchedule.Where(s => s.AppointmentDate == model.ByDate).ToList();
                        model.AppointmentSchedule = sortedByDate;
                        return View(model);
                    }
                    ViewBag.DoctorId = userId;
                    model.AppointmentSchedule = doctorSchedule;
                    SuccessNotification("Schedule updated successfully.");
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
        }

        public JsonResult ChangeBookingStatus(string type, string id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(type))
                {
                    if (Request.IsAuthenticated && User.IsInRole("Doctor"))
                    {
                        var appoinment = _scheduleService.GetAppointmentScheduleById(int.Parse(id));
                        if (appoinment == null)
                            return Json(new { action_type = "none" });

                        AppointmentStatus status;
                        if (type.Equals("approve"))
                        {
                            status = _scheduleService.GetAppointmentStatusByName("Booked");
                            if (status == null)
                                return Json(new { action_type = "none" });

                            appoinment.StatusId = status.Id;
                            _scheduleService.UpdateAppointmentSchedule(appoinment);
                            return Json(new
                            {
                                action_type = "approved"
                            });
                        }
                        else if (type.Equals("cancel"))
                        {
                            status = _scheduleService.GetAppointmentStatusByName("Cancelled");
                            if (status == null)
                                return Json(new { action_type = "none" });

                            appoinment.StatusId = status.Id;
                            _scheduleService.UpdateAppointmentSchedule(appoinment);

                            string userId = User.Identity.GetUserId();

                            var patient = context.AspNetUsers.Where(p => p.Id == appoinment.PatientId).FirstOrDefault();
                            var doc = context.Doctors.Where(p => p.DoctorId == userId).FirstOrDefault();
                            //Patient alert
                            if (!string.IsNullOrWhiteSpace(patient.PhoneNumber))
                            {
                                string msg = string.Empty;
                                msg += "CANCEL Appoinment ID:" + appoinment.Id;
                                msg += " for " + appoinment.AppointmentDate + " at time " + appoinment.AppointmentTime;
                                msg += " with Dr." + doc.AspNetUser.FirstName + " " + doc.AspNetUser.LastName;
                                msg += " Ph:" + doc.AspNetUser.PhoneNumber;
                                SendOtp(patient.PhoneNumber, msg);
                            }
                            //Doctor Alert
                            if (!string.IsNullOrWhiteSpace(doc.AspNetUser.PhoneNumber))
                            {
                                string msg = string.Empty;
                                msg += "CANCEL Appoinment ID:" + appoinment.Id;
                                msg += " for " + appoinment.AppointmentDate + " at time " + appoinment.AppointmentTime;
                                msg += " Patient:" + patient.FirstName + " " + patient.LastName;
                                msg += " Ph:" + patient.PhoneNumber;
                                SendOtp(patient.PhoneNumber, msg);
                            }

                            return Json(new
                            {
                                action_type = "cancelled"
                            });


                        }
                        else if (type.Equals("visit"))
                        {
                            status = _scheduleService.GetAppointmentStatusByName("Visited");
                            if (status == null)
                                return Json(new { action_type = "none" });

                            appoinment.StatusId = status.Id;
                            _scheduleService.UpdateAppointmentSchedule(appoinment);
                            return Json(new
                            {
                                action_type = "visited"
                            });
                        }
                        else if (type.Equals("failed"))
                        {
                            status = _scheduleService.GetAppointmentStatusByName("Failed");
                            if (status == null)
                                return Json(new { action_type = "none" });

                            appoinment.StatusId = status.Id;
                            _scheduleService.UpdateAppointmentSchedule(appoinment);
                            return Json(new
                            {
                                action_type = "visited"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Todo Log it
            }
            return Json(new
            {
                action_type = "none"
            });
        }

        public ActionResult BookingSchedules()
        {
            return View();
        }

        public ActionResult BookingSettings()
        {
            return View();
        }

        #endregion

        #region Profile Image

        public ContentResult UploadFiles()
        {
            try
            {
                var r = new List<UploadFilesResult>();
                var prodId = string.Empty;
                foreach (string file in Request.Files)
                {
                    var hpf = Request.Files[file];
                    if (hpf != null && hpf.ContentLength == 0)
                        continue;
                    if (hpf == null) continue;
                    prodId = Request.QueryString["pid"] + Path.GetExtension(hpf.FileName);
                    var folderPath = Server.MapPath("~/Uploads");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    var savedFileName = Path.Combine(folderPath, prodId);
                    hpf.SaveAs(savedFileName);

                    r.Add(new UploadFilesResult
                    {
                        Name = hpf.FileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType
                    });
                }

                return
                    Content(
                        "{\"name\":\"" + prodId + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" +
                        $"{r[0].Length} bytes" + "\"}", "application/json");
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Details

        public ActionResult DoctorDetails(string doctorId)
        {
            var model = new DoctorViewModel();
            if (!string.IsNullOrWhiteSpace(doctorId))
            {
                var doctor = _doctorService.GetDoctorbyId(doctorId);
                if (doctor == null)
                    return null;

                var user = GetUserById(doctor.DoctorId);
                if (user == null)
                    return null;

                model.Id = user.Id;
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.MobileNumber = user.PhoneNumber;
                model.doctor = doctor;
                model.Addresses = _addressService.GetAllAddressByUser(doctor.DoctorId);
                model.Specialitys = _specialityService.GetDoctorSpecilities(doctor.DoctorId);
                model.Schedule = _scheduleService.GetScheduleByDoctorId(doctor.DoctorId);
                model.listReviewComments = _reviewCommentsService.GetAllAprovedReviewCommentsByUser(doctor.DoctorId);

                #region Picture
                var pictures = _pictureService.GetPicturesByUserId(doctorId);
                var defaultPicture = pictures.FirstOrDefault();
                var defaultPictureModel = new PictureModel
                {
                    ImageUrl = _pictureService.GetPictureUrl(defaultPicture, 365, false),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(defaultPicture, 0, false),
                    Title = "",
                    AlternateText = "",
                };

                //all pictures
                var pictureModels = new List<PictureModel>();
                foreach (var picture in pictures)
                {
                    var pictureModel = new PictureModel
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture, 150),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                        Title = "",
                        AlternateText = "",
                    };
                    pictureModels.Add(pictureModel);
                }
                model.AddPictureModel = defaultPictureModel;
                model.DoctorPictureModels = pictureModels;
                #endregion

                TempData["CommentForId"] = doctorId;
                ViewBag.User = User.Identity.GetUserId();
                return View(model);
            }
            return View();
        }

        #endregion

        #region Review Comments
        [HttpPost]
        public ActionResult SaveReivewComment(FormCollection form)
        {
            ReviewComments ReviewComments = new ReviewComments();
            ReviewComments.CommentForId = TempData["CommentForId"].ToString();
            ReviewComments.CommentOwnerId = User.Identity.GetUserId();
            ReviewComments.CommentOwnerUser = User.Identity.Name;
            ReviewComments.Comment = form["UserComment"];
            ReviewComments.Rating = Convert.ToDecimal(form["starrating"]) * 20;
            ReviewComments.DateCreated = DateTime.Now;
            ReviewComments.IsApproved = false;
            ReviewComments.IsActive = true;

            if (_reviewCommentsService.InsertReviewComment(ReviewComments))
                return Content("Success");

            return Content("Error");
        }
        #endregion

        #region Social Links

        [HttpPost]
        public ActionResult SocialLink_Read(DataSourceRequest command, string docterId)
        {
            var socialSites = _doctorService.GetAllLinksByDoctor(docterId, command.Page - 1, 5, false);
            var gridModel = new DataSourceResult
            {
                Data = socialSites.Select(x => new SocialLinkInformation
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    DisplayOrder = x.DisplayOrder,
                    DoctorId = x.DoctorId,
                    SocialLink = x.SocialLink,
                    SocialType = x.SocialType
                }),
                Total = socialSites.TotalCount
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult SocialLink_Add([Bind(Exclude = "Id")] SocialLinkInformation model, string docterId)
        {
            try
            {
                if (model.SocialType != null)
                    model.SocialType = model.SocialType.Trim();
                if (model.SocialLink != null)
                    model.SocialLink = model.SocialLink.Trim();
                if (docterId != null)
                    model.DoctorId = docterId;

                if (!ModelState.IsValid && model.DoctorId == null)
                {
                    return Json(new DataSourceResult { Errors = "error" });
                }
                var link =
                    _doctorService.GetAllLinksByDoctor(docterId).FirstOrDefault(c => c.SocialType == model.SocialType);
                if (link == null)
                {
                    _doctorService.InsertSocialLink(model);
                }
                return new NullJsonResult();
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }
        }

        [HttpPost]
        public ActionResult SocialLink_Update(SocialLinkInformation model, string docterId)
        {
            try
            {
                var link = _doctorService.GetSocialLinkbyId(model.Id);
                if (link == null)
                    return Content("No link could be loaded with the specified ID");

                if (!link.SocialType.Equals(model.SocialType, StringComparison.InvariantCultureIgnoreCase) ||
                    link.Id != model.Id)
                {
                    _doctorService.DeleteSocialLink(link);
                }

                link.Id = model.Id;
                link.DoctorId = model.DoctorId;
                link.SocialType = model.SocialType;
                link.SocialLink = model.SocialLink;
                link.IsActive = model.IsActive;
                link.DisplayOrder = model.DisplayOrder;
                _doctorService.UpdateSocialLink(link);
                return new NullJsonResult();
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }
        }

        [HttpPost]
        public ActionResult SocialLink_Delete(int id)
        {
            try
            {
                var link = _doctorService.GetSocialLinkbyId(id);
                if (link == null)
                    throw new ArgumentException("No link found with the specified id");
                _doctorService.DeleteSocialLink(link);

                return new NullJsonResult();
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }
        }

        #endregion

        #region Honors & Awards

        [HttpPost]
        public ActionResult HonorsAwards_Read(DataSourceRequest command, string docterId)
        {
            try
            {
                var awardsHonars = _doctorService.GetAllHonorsAwards(docterId, command.Page - 1, 5, false);
                var gridModel = new DataSourceResult
                {
                    Data = awardsHonars.Select(x => new HonorsAwards
                    {
                        Id = x.Id,
                        IsActive = x.IsActive,
                        DisplayOrder = x.DisplayOrder,
                        DoctorId = x.DoctorId,
                        Name = x.Name,
                        Description = x.Description,
                        AwardDate = x.AwardDate
                    }),
                    Total = awardsHonars.TotalCount
                };
                return Json(gridModel);
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }
        }

        [HttpPost]
        public ActionResult HonorsAwards_Add([Bind(Exclude = "Id")] HonorsAwards awards, string docterId)
        {
            try
            {
                if (awards.Name != null)
                    awards.Name = awards.Name.Trim();
                if (awards.Description != null)
                    awards.Description = awards.Description.Trim();
                if (docterId != null)
                    awards.DoctorId = docterId;

                if (!ModelState.IsValid && awards.DoctorId == null)
                {
                    return Json(new DataSourceResult { Errors = "error" });
                }

                _doctorService.InsertHonorsAwards(awards);

                return new NullJsonResult();
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }
        }

        [HttpPost]
        public ActionResult HonorsAwards_Update(HonorsAwards awards, string docterId)
        {
            try
            {
                var award = _doctorService.GetHonorsAwardsbyId(awards.Id);
                if (award == null)
                    return Content("No link could be loaded with the specified ID");

                if (!award.Name.Equals(awards.Name, StringComparison.InvariantCultureIgnoreCase) ||
                    award.Id != awards.Id)
                {
                    _doctorService.DeleteHonorsAwards(award);
                }

                award.Id = awards.Id;
                award.DoctorId = awards.DoctorId;
                award.Name = awards.Name;
                award.Description = awards.Description;
                award.AwardDate = awards.AwardDate;
                award.IsActive = awards.IsActive;
                award.DisplayOrder = awards.DisplayOrder;
                _doctorService.UpdateHonorsAwards(award);

                return new NullJsonResult();
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }

        }

        [HttpPost]
        public ActionResult HonorsAwards_Delete(HonorsAwards awards)
        {
            try
            {
                if (awards.Id > 0)
                {
                    var award = _doctorService.GetHonorsAwardsbyId(awards.Id);
                    if (award == null)
                        throw new ArgumentException("No Awards found with the specified id");
                    _doctorService.DeleteHonorsAwards(award);
                }

                return new NullJsonResult();
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }
        }
        #endregion

        #region Education
        [HttpPost]
        public ActionResult Education_Read(DataSourceRequest command, string docterId)
        {
            var education = _doctorService.GetAllEducation(docterId, command.Page - 1, 5, false);
            var gridModel = new DataSourceResult
            {
                Data = education.Select(x => new Education
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    DisplayOrder = x.DisplayOrder,
                    DoctorId = x.DoctorId,
                    Title = x.Title,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Institute = x.Institute
                }),
                Total = education.TotalCount
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult Education_Add([Bind(Exclude = "Id")] Education education, string docterId)
        {
            if (education.Title != null)
                education.Title = education.Title.Trim();
            if (education.Description != null)
                education.Description = education.Description.Trim();
            if (education.Institute != null)
                education.Institute = education.Institute.Trim();
            if (docterId != null)
                education.DoctorId = docterId;

            if (!ModelState.IsValid && education.DoctorId == null)
            {
                return Json(new DataSourceResult { Errors = "error" });
            }

            _doctorService.InsertEducation(education);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult Education_Update(Education education, string docterId)
        {
            var edu = _doctorService.GetEducationbyId(education.Id);
            if (edu == null)
                return Content("No education could be loaded with the specified ID");

            if (!education.Title.Equals(edu.Title, StringComparison.InvariantCultureIgnoreCase) ||
                education.Id != edu.Id)
            {
                _doctorService.DeleteEducation(edu);
            }

            edu.Id = education.Id;
            edu.DoctorId = education.DoctorId;
            edu.Title = education.Title;
            edu.Description = education.Description;
            edu.IsActive = education.IsActive;
            edu.DisplayOrder = education.DisplayOrder;
            edu.Institute = education.Institute;
            edu.StartDate = education.StartDate;
            edu.EndDate = education.EndDate;
            _doctorService.UpdateEducation(edu);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult Education_Delete(Education education)
        {
            if (education.Id > 0)
            {
                var edu = _doctorService.GetEducationbyId(education.Id);
                if (edu == null)
                    throw new ArgumentException("No education found with the specified id");
                _doctorService.DeleteEducation(edu);
            }

            return new NullJsonResult();
        }
        #endregion

        #region Experience
        [HttpPost]
        public ActionResult Experience_Read(DataSourceRequest command, string docterId)
        {
            var experience = _doctorService.GetAllExperience(docterId, command.Page - 1, 5, false);
            var gridModel = new DataSourceResult
            {
                Data = experience.Select(x => new Experience
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    DisplayOrder = x.DisplayOrder,
                    DoctorId = x.DoctorId,
                    Title = x.Title,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Organization = x.Organization
                }),
                Total = experience.TotalCount
            };
            return Json(gridModel);
        }
        [HttpPost]
        public ActionResult Experience_Add([Bind(Exclude = "Id")] Experience experience, string docterId)
        {
            if (experience.Title != null)
                experience.Title = experience.Title.Trim();
            if (experience.Description != null)
                experience.Description = experience.Description.Trim();
            if (experience.Organization != null)
                experience.Organization = experience.Organization.Trim();
            if (docterId != null)
                experience.DoctorId = docterId;

            if (!ModelState.IsValid && experience.DoctorId == null)
            {
                return Json(new DataSourceResult { Errors = "error" });
            }

            _doctorService.InsertExperience(experience);

            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult Experience_Update(Experience experience, string docterId)
        {
            var expr = _doctorService.GetExperiencebyId(experience.Id);
            if (expr == null)
                return Content("No Experience could be loaded with the specified ID");

            if (!experience.Title.Equals(expr.Title, StringComparison.InvariantCultureIgnoreCase) ||
                experience.Id != expr.Id)
            {
                _doctorService.DeleteExperience(expr);
            }

            expr.Id = experience.Id;
            expr.DoctorId = experience.DoctorId;
            expr.Title = experience.Title;
            expr.Description = experience.Description;
            expr.IsActive = experience.IsActive;
            expr.DisplayOrder = experience.DisplayOrder;
            expr.Organization = experience.Organization;
            expr.StartDate = experience.StartDate;
            expr.EndDate = experience.EndDate;
            _doctorService.UpdateExperience(expr);

            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult Experience_Delete(Experience experience)
        {
            if (experience.Id > 0)
            {
                var expr = _doctorService.GetExperiencebyId(experience.Id);
                if (expr == null)
                    throw new ArgumentException("No Experience found with the specified id");
                _doctorService.DeleteExperience(expr);
            }

            return new NullJsonResult();
        }
        #endregion

        #region Picture
        [ValidateInput(false)]
        public ActionResult DoctorPictureAdd(int pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute,
            string doctorId)
        {
            if (pictureId == 0)
                throw new ArgumentException();

            var doctor = _doctorService.GetDoctorbyId(doctorId);
            if (doctor == null)
                throw new ArgumentException("No doctor found with the specified id");

            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            _doctorService.InsertDoctorPicture(new PictureMapping
            {
                PictureId = pictureId,
                UserId = doctorId,
                DisplayOrder = displayOrder,
            });

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                overrideAltAttribute,
                overrideTitleAttribute);

           // _pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(doctor.AspNetUser.Id));

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DoctorShowcasePictures()
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var model = new DoctorPictureModel();
            model.DoctorId = User.Identity.GetUserId();
            return View(model);
        }
        [HttpPost]
        public ActionResult DoctorPictureList(DataSourceRequest command, string doctorId)
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var doctorPictures = _doctorService.GetDoctorPicturesByDoctorId(doctorId);
            var doctorPicturesModel = doctorPictures
                .Select(x =>
                {
                    var picture = _pictureService.GetPictureById(x.PictureId);
                    if (picture == null)
                        throw new Exception("Picture cannot be loaded");
                    var m = new DoctorPictureModel
                    {
                        Id = x.Id,
                        DoctorId = x.UserId,
                        PictureId = x.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(picture),
                        OverrideAltAttribute = picture.AltAttribute,
                        OverrideTitleAttribute = picture.TitleAttribute,
                        DisplayOrder = x.DisplayOrder
                    };
                    return m;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = doctorPicturesModel,
                Total = doctorPicturesModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult DoctorPictureUpdate(DoctorPictureModel model)
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var doctorPicture = _doctorService.GetDoctorPictureById(model.Id);
            if (doctorPicture == null)
                throw new ArgumentException("No Doctor picture found with the specified id");
           
            doctorPicture.DisplayOrder = model.DisplayOrder;
            _doctorService.UpdateDoctorPicture(doctorPicture);

            var picture = _pictureService.GetPictureById(doctorPicture.PictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                model.OverrideAltAttribute,
                model.OverrideTitleAttribute);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult DoctorPictureDelete(int id)
        {
            if (!Request.IsAuthenticated && !User.IsInRole("Doctor"))
                return new HttpUnauthorizedResult();

            var doctorPicture = _doctorService.GetDoctorPictureById(id);
            if (doctorPicture == null)
                throw new ArgumentException("No doctor picture found with the specified id");

            var doctorId = doctorPicture.UserId;

            var pictureId = doctorPicture.PictureId;
            _doctorService.DeleteDoctorPicture(doctorPicture);

            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");
            _pictureService.DeletePicture(picture);

            return new NullJsonResult();
        }
        #endregion



    }
}