using DPTS.Common.Kendoui;
using DPTS.Domain.Core.Common;
using DPTS.Domain.Core.Doctors;
using DPTS.Domain.Entities;
using DPTS.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    public class AdministrationController : Controller
    {
        #region Fields
        private readonly IDoctorService _doctorService;
        private readonly IQualifiactionService _qualificationService;
        #endregion

        #region Ctr
        public AdministrationController(IDoctorService doctorService,
            IQualifiactionService qualificationService)
        {
            _doctorService = doctorService;
            _qualificationService = qualificationService;
        }
        #endregion

        // GET: Administration
        [Route("/admin")]
        public ActionResult Index()
        {
            return View();
        }

        #region Doctor
        public ActionResult Doctors()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Doctor_Read(DataSourceRequest command)
        //{
        //    var doctor = _doctorService.GetAllDoctors(command.Page - 1, 5, );
        //    var gridModel = new DataSourceResult
        //    {
        //        Data = doctor.Select(x => new Doctor
        //        {
        //            Id = x.Id,
        //            IsActive = x.IsActive,
        //            DisplayOrder = x.DisplayOrder,
        //            DoctorId = x.DoctorId,
        //            Title = x.Title,
        //            Description = x.Description,
        //            StartDate = x.StartDate,
        //            EndDate = x.EndDate,
        //            Organization = x.Organization
        //        }),
        //        Total = experience.TotalCount
        //    };
        //    return Json(gridModel);
        //}
        #endregion

        #region Qualification Grid

        public ActionResult QualifiactionList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Qualification_Read(DataSourceRequest command)
        {
            var qualification = _qualificationService.GetAllQualifiaction(command.Page - 1, 10, false);
            var gridModel = new DataSourceResult
            {
                Data = qualification.Select(x => new Qualifiaction
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    DisplayOrder = x.DisplayOrder,
                    Name = x.Name
                }),
                Total = qualification.TotalCount
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult Qualification_Add([Bind(Exclude = "Id")] Qualifiaction model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new DataSourceResult { Errors = "error" });
                }
                var qualification =
                    _qualificationService.GetAllQualifiaction().FirstOrDefault(c => c.Name == model.Name);
                if (qualification == null)
                {
                    _qualificationService.AddQualifiaction(model);
                }
                return new NullJsonResult();
            }
            catch (Exception ex)
            {
                return new NullJsonResult();
            }
        }

        [HttpPost]
        public ActionResult Qualification_Update(Qualifiaction model)
        {
            try
            {
                var qualification = _qualificationService.GetQualifiactionbyId(model.Id);
                if (qualification == null)
                    return Content("No link could be loaded with the specified ID");

                if (!qualification.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase) ||
                    qualification.Id != model.Id)
                {
                    _qualificationService.DeleteQualifiaction(qualification);
                }

                qualification.Id = model.Id;
                qualification.Name = model.Name;
                qualification.IsActive = model.IsActive;
                qualification.DisplayOrder = model.DisplayOrder;
                _qualificationService.UpdateQualifiaction(qualification);
                return new NullJsonResult();
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }
        }

        [HttpPost]
        public ActionResult Qualification_Delete(int id)
        {
            try
            {
                var qualification = _qualificationService.GetQualifiactionbyId(id);
                if (qualification == null)
                    throw new ArgumentException("No link found with the specified id");
                _qualificationService.DeleteQualifiaction(qualification);

                return new NullJsonResult();
            }
            catch (Exception)
            {
                return new NullJsonResult();
            }
        }
        #endregion
    }
}