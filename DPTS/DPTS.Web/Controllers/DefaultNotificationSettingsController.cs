using DPTS.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DPTS.Domain.Core.Notification;
using DPTS.Domain.Entities.Notification;

namespace DPTS.Web.Controllers
{
    [Authorize]
    public class DefaultNotificationSettingsController : BaseController
    {
        #region Fields

        private readonly IDefaultNotificationSettingsService _defaultNotificationSettingsService;
        private readonly IEmailCategoryService _emailCategoryService;

        #endregion

        #region Contructor

        public DefaultNotificationSettingsController(
            IDefaultNotificationSettingsService defaultNotificationSettingsService,
            IEmailCategoryService emailCategoryService)
        {
            _defaultNotificationSettingsService = defaultNotificationSettingsService;
            _emailCategoryService = emailCategoryService;
        }

        #endregion

        #region Utilities

        public IList<SelectListItem> GetDefaultNotificationSettings()
        {
            var emailCategories = _emailCategoryService.GetAllEmailCategories(true);
            var typelst = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                }
            };
            typelst.AddRange(emailCategories.ToList().Select(type => new SelectListItem
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
            var defaultNotificationSettings = _defaultNotificationSettingsService.GetAllDefaultNotificationSettings(true);
            var model = defaultNotificationSettings.Select(c => new DefaultNotificationSettingsViewModel
            {
                Id = c.Id,
                Name = c.Name,
                EmailCategory = _emailCategoryService.GetEmailCategoryById(c.Id).Name,
                DisplayOrder = c.DisplayOrder,
                Message = c.Message,
                Published = c.Published,
                EmailCategoryId = c.CategoryId
            }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new DefaultNotificationSettingsViewModel
            {
                AvailableEmailCategory = GetDefaultNotificationSettings()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DefaultNotificationSettingsViewModel model)
        {
            if (model.EmailCategoryId == 0)
                ModelState.AddModelError("", "select Email Category");

            if (ModelState.IsValid)
            {
                var stateProvince = new DefaultNotificationSettings
                {
                    Name = model.Name,
                    Published = model.Published,
                    DisplayOrder = model.DisplayOrder,
                    Message = model.Message,
                    CategoryId = model.EmailCategoryId
                };
                _defaultNotificationSettingsService.InsertDefaultNotificationSettings(stateProvince);
                return RedirectToAction("List");
            }
            model.AvailableEmailCategory = GetDefaultNotificationSettings();
            return View(model);
        }

        public ActionResult Edit(int Id)
        {
            if (!IsValidateId(Id))
                return HttpNotFound();

            var defaultNotificationSettings = _defaultNotificationSettingsService.GetDefaultNotificationSettingsById(Id);
            if (defaultNotificationSettings == null)
                return HttpNotFound();

            var model = new DefaultNotificationSettingsViewModel
            {
                Id = defaultNotificationSettings.Id,
                Name = defaultNotificationSettings.Name,
                EmailCategory = _emailCategoryService.GetEmailCategoryById(defaultNotificationSettings.Id).Name,
                DisplayOrder = defaultNotificationSettings.DisplayOrder,
                Published = defaultNotificationSettings.Published,
                EmailCategoryId = defaultNotificationSettings.CategoryId,
                Message = defaultNotificationSettings.Message,
                AvailableEmailCategory = GetDefaultNotificationSettings()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DefaultNotificationSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var stateProvince = _defaultNotificationSettingsService.GetDefaultNotificationSettingsById(model.Id);
                stateProvince.Id = model.Id;
                stateProvince.Name = model.Name;
                stateProvince.DisplayOrder = model.DisplayOrder;
                stateProvince.Published = model.Published;
                stateProvince.CategoryId = model.EmailCategoryId;
                stateProvince.Message = model.Message;

                _defaultNotificationSettingsService.UpdateDefaultNotificationSettings(stateProvince);
                return RedirectToAction("List");
            }
            model.AvailableEmailCategory = GetDefaultNotificationSettings();
            return View(model);
        }

        public ActionResult DeleteConfirmed(int id)
        {
            var defaultNotificationSettings = _defaultNotificationSettingsService.GetDefaultNotificationSettingsById(id);
            if (defaultNotificationSettings != null)
                _defaultNotificationSettingsService.DeleteDefaultNotificationSettings(defaultNotificationSettings);


            return Content("Deleted");
        }

        #endregion
    }
}