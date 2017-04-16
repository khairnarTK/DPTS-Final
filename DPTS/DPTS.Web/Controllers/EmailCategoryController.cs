using DPTS.Web.Models;
using System.Linq;
using System.Web.Mvc;
using DPTS.Domain.Core.Notification;
using DPTS.Domain.Entities.Notification;

namespace DPTS.Web.Controllers
{
    [Authorize]
    public class EmailCategoryController : BaseController
    {
        #region Fields

        private readonly IEmailCategoryService _emailCategory;

        #endregion

        #region Contructor

        public EmailCategoryController(IEmailCategoryService emailCategoryService)
        {
            _emailCategory = emailCategoryService;
        }

        #endregion

        #region Methods

        public ActionResult List()
        {
            var countries = _emailCategory.GetAllEmailCategories(true);
            var model = countries.Select(c => new EmailCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                DisplayOrder = c.DisplayOrder,
                Published = c.Published
            }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new EmailCategoryViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(EmailCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var country = new EmailCategory
                {
                    Name = model.Name,
                    Published = model.Published,
                    DisplayOrder = model.DisplayOrder
                };
                _emailCategory.InsertEmailCategory(country);
                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult Edit(int Id)
        {
            if (!IsValidateId(Id))
                return HttpNotFound();

            var country = _emailCategory.GetEmailCategoryById(Id);
            if (country == null)
                return HttpNotFound();

            var model = new EmailCategoryViewModel
            {
                Id = country.Id,
                Name = country.Name,
                DisplayOrder = country.DisplayOrder,
                Published = country.Published
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmailCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var country = _emailCategory.GetEmailCategoryById(model.Id);
                country.Id = model.Id;
                country.Name = model.Name;
                country.DisplayOrder = model.DisplayOrder;
                country.Published = model.Published;
                _emailCategory.UpdateEmailCategory(country);
                return RedirectToAction("List");
            }
            return View();
        }


        public ActionResult DeleteConfirmed(int id)
        {
            var country = _emailCategory.GetEmailCategoryById(id);
            if (country != null)
                _emailCategory.DeleteEmailCategory(country);


            return Content("Deleted");
        }

        #endregion
    }
}