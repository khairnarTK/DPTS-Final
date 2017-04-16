using System;
using System.Web.Mvc;
using DPTS.Domain.Core.Speciality;
using DPTS.Domain.Entities;
using DPTS.Common.Kendoui;
using System.Linq;

namespace DPTS.Web.Controllers
{
    public class SpecialityController : BaseController
    {
        #region Field

        private readonly ISpecialityService _specialityService;

        #endregion

        #region Constructor

        public SpecialityController(ISpecialityService specialityService)
        {
            _specialityService = specialityService;
        }

        #endregion

        #region utitlity

        [NonAction]
        protected bool IsValidateId(int id)
        {
            return id != 0;
        }

        #endregion

        #region Methods

        public ActionResult List()
        {
            var model = _specialityService.GetAllSpeciality(true);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new Speciality();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Speciality model)
        {
            if (ModelState.IsValid)
            {
                var speciality = new Speciality
                {
                    Title = model.Title,
                    DisplayOrder = model.DisplayOrder,
                    IsActive = model.IsActive,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                };
                _specialityService.AddSpeciality(speciality);
                SuccessNotification("Speciality added successfully.");
                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!IsValidateId(id))
                return null;

            var speciality = _specialityService.GetSpecialitybyId(id);
            if (speciality == null)
                return null;

            var model = new Speciality
            {
                Id = speciality.Id,
                Title = speciality.Title,
                IsActive = speciality.IsActive,
                DisplayOrder = speciality.DisplayOrder,
                DateUpdated = DateTime.UtcNow
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Speciality model)
        {
            if (ModelState.IsValid)
            {
                var speciality = _specialityService.GetSpecialitybyId(model.Id);
                speciality.Title = model.Title;
                speciality.DisplayOrder = model.DisplayOrder;
                speciality.IsActive = model.IsActive;
                speciality.DateCreated = DateTime.UtcNow;
                speciality.DateUpdated = DateTime.UtcNow;
                _specialityService.UpdateSpeciality(speciality);
                SuccessNotification("Speciality updated successfully.");
                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            var speciality = _specialityService.GetSpecialitybyId(id);
            if (speciality != null)
            {
                _specialityService.DeleteSpeciality(speciality);
                SuccessNotification("Speciality deleted successfully.");
            }


            return Content("Deleted");
        }

        //Search autocomplete call
        public JsonResult Speciality_Read()
        {
            var specialities = _specialityService.GetAllSpeciality(true);
            var data = specialities.Select(x => new Speciality
            {
                Title = x.Title
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}