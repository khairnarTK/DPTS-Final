using DPTS.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DPTS.Domain.Core.Country;
using DPTS.Domain.Core.StateProvince;
using DPTS.Domain.Entities;

namespace DPTS.Web.Controllers
{
    [Authorize]
    public class StateProvinceController : BaseController
    {
        #region Fields

        private readonly IStateProvinceService _stateProvinceService;
        private readonly ICountryService _countryService;

        #endregion

        #region Contructor

        public StateProvinceController(IStateProvinceService stateProvinceService, ICountryService countryService)
        {
            _stateProvinceService = stateProvinceService;
            _countryService = countryService;
        }

        #endregion

        #region Utilities

        public IList<SelectListItem> GetCountryList()
        {
            var countries = _countryService.GetAllCountries(true);
            List<SelectListItem> typelst = new List<SelectListItem>
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

        #endregion

        #region Methods

        public ActionResult List()
        {
            var countries = _stateProvinceService.GetAllStateProvince();
            var model = countries.Select(c => new StateProvinceViewModel
            {
                Id = c.Id,
                Name = c.Name,
                CountryName = _countryService.GetCountryById(c.CountryId).Name,
                DisplayOrder = c.DisplayOrder,
                Abbreviation = c.Abbreviation,
                Published = c.Published,
                CountryId = c.CountryId
            }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new StateProvinceViewModel();
            model.AvailableCountry = GetCountryList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(StateProvinceViewModel model)
        {
            try
            {
                if (model.CountryId == 0)
                    ModelState.AddModelError("", "select country");

                if (ModelState.IsValid)
                {
                    var stateProvince = new StateProvince
                    {
                        Name = model.Name,
                        Published = model.Published,
                        DisplayOrder = model.DisplayOrder,
                        Abbreviation = model.Abbreviation,
                        CountryId = model.CountryId
                    };
                    _stateProvinceService.InsertStateProvince(stateProvince);
                    SuccessNotification("State added successfully.");
                    return RedirectToAction("List");
                }
                model.AvailableCountry = GetCountryList();
                ErrorNotification("Fill required fields");
                return View(model);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                if (!IsValidateId(Id))
                    return HttpNotFound();

                var stateProvince = _stateProvinceService.GetStateProvinceById(Id);
                if (stateProvince == null)
                    return HttpNotFound();

                var model = new StateProvinceViewModel
                {
                    Id = stateProvince.Id,
                    Name = stateProvince.Name,
                    CountryName = _countryService.GetCountryById(stateProvince.CountryId).Name,
                    DisplayOrder = stateProvince.DisplayOrder,
                    Published = stateProvince.Published,
                    CountryId = stateProvince.CountryId,
                    Abbreviation = stateProvince.Abbreviation,
                    AvailableCountry = GetCountryList()
                };
                return View(model);
            }
            catch { throw; }
        }

        [HttpPost]
        public ActionResult Edit(StateProvinceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var stateProvince = _stateProvinceService.GetStateProvinceById(model.Id);
                stateProvince.Id = model.Id;
                stateProvince.Name = model.Name;
                stateProvince.DisplayOrder = model.DisplayOrder;
                stateProvince.Published = model.Published;
                stateProvince.CountryId = model.CountryId;
                stateProvince.Abbreviation = model.Abbreviation;
                _stateProvinceService.UpdateStateProvince(stateProvince);
                SuccessNotification("State updated successfully.");
                return RedirectToAction("List");
            }
            model.AvailableCountry = GetCountryList();
            ErrorNotification("Fill required fields");
            return View(model);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            var stateProvince = _stateProvinceService.GetStateProvinceById(id);
            if (stateProvince != null)
                _stateProvinceService.DeleteStateProvince(stateProvince);


            return Content("Deleted");
        }

        #endregion
    }
}