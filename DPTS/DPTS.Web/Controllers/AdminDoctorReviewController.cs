using DPTS.Common.Kendoui;
using DPTS.Domain.Core.Doctors;
using DPTS.Domain.Entities;
using DPTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    public class AdminDoctorReviewController : BaseController
    {
        #region Fields

        private readonly IDoctorService _doctorService;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        #endregion Fields

        #region Constructors

        public AdminDoctorReviewController(IDoctorService doctorService)
        {
            this._doctorService = doctorService;
        }

        #endregion

        #region Utilities
        public virtual DateTime ConvertToUserTime(DateTime dt, DateTimeKind sourceDateTimeKind)
        {
            dt = DateTime.SpecifyKind(dt, sourceDateTimeKind);
            return TimeZoneInfo.ConvertTime(dt, INDIAN_ZONE);
        }

        [NonAction]
        protected virtual void PrepareDoctorReviewModel(AdminDocorReviewModel model,
            DoctorReview doctorReview, bool excludeProperties, bool formatReviewText)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (doctorReview == null)
                throw new ArgumentNullException("doctorReview");

            model.Id = doctorReview.Id;
            model.DoctorId = doctorReview.DoctorId;
            model.DoctorName = "Dr."+ doctorReview.Doctor.AspNetUser.FirstName+ " "+ doctorReview.Doctor.AspNetUser.LastName;
            model.PatientId = doctorReview.PatientId;
            var patient = doctorReview.Patient;

            model.PatientInfo = (patient != null) ? patient.FirstName + " " + patient.LastName : "None";
           // model.PatientInfo = patient.IsRegistered() ? patient.Email : _localizationService.GetResource("Admin.Customers.Guest");
            model.Rating = doctorReview.Rating;
            model.ReplyText = doctorReview.ReplyText;
            model.CreatedOn = ConvertToUserTime(doctorReview.CreatedOnUtc, DateTimeKind.Utc);
            if (!excludeProperties)
            {
                model.Title = doctorReview.Title;
                model.ReviewText = doctorReview.ReviewText;
                model.IsApproved = doctorReview.IsApproved;
            }
        }

        #endregion

        #region Methods

        //list
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            //if (!User.IsInRole("Admin"))
            //    return AccessDeniedView();
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            var productReviews = _doctorService.GetAlldoctorReviews(null,null);
            var gridModel = new DataSourceResult
            {
                Data = productReviews.Select(x =>
                {
                    var m = new AdminDocorReviewModel();
                    PrepareDoctorReviewModel(m, x, false, true);
                    return m;
                }),
                Total = productReviews.TotalCount
            };

            return Json(gridModel);
        }

        //edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var doctorReview = _doctorService.GetDoctorReviewById(id);
            if (doctorReview == null)
                return RedirectToAction("List");

            var model = new AdminDocorReviewModel();
            PrepareDoctorReviewModel(model, doctorReview, false, false);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdminDocorReviewModel model)
        {
            var doctorReview = _doctorService.GetDoctorReviewById(model.Id);
            if (doctorReview == null)
                //No product review found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                doctorReview.Title = model.Title;
                doctorReview.ReviewText = model.ReviewText;
                doctorReview.IsApproved = model.IsApproved;
                doctorReview.ReplyText = model.ReplyText;
                _doctorService.UpdateDoctorReview(doctorReview);

                //update product totals
                _doctorService.UpdateDoctorReviewTotals(doctorReview.Doctor);

                SuccessNotification("Review updated Successfully.");

                return RedirectToAction("List");
            }


            //If we got this far, something failed, redisplay form
            PrepareDoctorReviewModel(model, doctorReview, true, false);
            return View(model);
        }

        //delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var doctorReview = _doctorService.GetDoctorReviewById(id);
            if (doctorReview == null)
                //No product review found with the specified id
                return RedirectToAction("List");

            var doctor = doctorReview.Doctor;
            _doctorService.DeleteDoctorReview(doctorReview);
            //update product totals
            _doctorService.UpdateDoctorReviewTotals(doctor);

            SuccessNotification("Review deleted successfully..");
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ApproveSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                var doctorReviews = _doctorService.GetDoctorReviewsByIds(selectedIds.ToArray());
                foreach (var doctorReview in doctorReviews)
                {
                    var previousIsApproved = doctorReview.IsApproved;
                    doctorReview.IsApproved = true;
                    _doctorService.UpdateDoctorReview(doctorReview);
                    //update product totals
                    _doctorService.UpdateDoctorReviewTotals(doctorReview.Doctor);
                }
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult DisapproveSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                var doctorReviews = _doctorService.GetDoctorReviewsByIds(selectedIds.ToArray());
                foreach (var doctorReview in doctorReviews)
                {
                    doctorReview.IsApproved = false;
                    _doctorService.UpdateDoctorReview(doctorReview);
                    //update product totals
                    _doctorService.UpdateDoctorReviewTotals(doctorReview.Doctor);
                }
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
            //    return AccessDeniedView();

            if (selectedIds != null)
            {
                var doctorReviews = _doctorService.GetDoctorReviewsByIds(selectedIds.ToArray());
                var doctors = _doctorService.GetDoctorsByIds(doctorReviews.Select(p => p.DoctorId).ToArray());

                _doctorService.DeleteDoctorReviews(doctorReviews);

                //update product totals
                foreach (var doctor in doctors)
                {
                    _doctorService.UpdateDoctorReviewTotals(doctor);
                }
            }

            return Json(new { Result = true });
        }

        //public ActionResult ProductSearchAutoComplete(string term)
        //{
        //    const int searchTermMinimumLength = 3;
        //    if (String.IsNullOrWhiteSpace(term) || term.Length < searchTermMinimumLength)
        //        return Content("");

        //    //products
        //    const int productNumber = 15;
        //    var products = _productService.SearchProducts(
        //        keywords: term,
        //        pageSize: productNumber,
        //        showHidden: true);

        //    var result = (from p in products
        //                  select new
        //                  {
        //                      label = p.Name,
        //                      productid = p.Id
        //                  })
        //        .ToList();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        #endregion
    }
}