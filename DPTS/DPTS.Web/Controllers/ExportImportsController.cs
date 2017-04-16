using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DPTS.Data.Context;
using DPTS.Domain.Core.Doctors;
using DPTS.Domain.Core.ExportImport;
using DPTS.Domain.Entities;
using DPTS.Services;
using DPTS.Services.ExportImport.Help;
using DPTS.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;

namespace DPTS.Web.Controllers
{
    [Authorize]
    public class ExportImportsController : BaseController
    {
        #region Feilds

        private ApplicationUserManager _userManager;
        private readonly IImportManager _importManager;
        private readonly IDoctorService _doctorService;
        private readonly DPTSDbContext _context;

        #endregion

        #region Contructor

        public ExportImportsController(IImportManager importManager,
            IDoctorService doctorService,
            DPTSDbContext context)
        {
            _importManager = importManager;
            _context = context;
            _doctorService = doctorService;
        }

        #endregion

        #region Utilities

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public string UppercaseFirstLetter(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<ActionResult> ImportDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ImportDoctors()
        {
            try
            {
                var file = Request.Files["importexcelfile"];
                if (file != null && file.ContentLength > 0)
                {
                    var properties = new[]
                    {
                        //user
                        new PropertyByName<AspNetUser>("FirstName"),
                        new PropertyByName<AspNetUser>("LastName"),
                        new PropertyByName<AspNetUser>("Email"),
                        new PropertyByName<AspNetUser>("PhoneNumber"),
                    };

                    var manager = new PropertyManager<AspNetUser>(properties);

                    using (var xlPackage = new ExcelPackage(file.InputStream))
                    {
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                            throw new DptsException("No worksheet found");

                        var iRow = 2;

                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[iRow, property.PropertyOrderPosition])
                                .All(
                                    cell =>
                                        cell == null || cell.Value == null ||
                                        String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, iRow);

                            var userEmail = manager.GetProperty("Email").StringValue;
                            var aspNetUser =
                                _context.AspNetUsers
                                    .Where(d => d.Email.Equals(userEmail)).FirstOrDefault();

                            var isNew = aspNetUser == null;

                            aspNetUser = aspNetUser ?? new AspNetUser();

                            if (isNew)
                                aspNetUser.CreatedOnUtc = DateTime.UtcNow;


                            var user = new ApplicationUser
                            {
                                UserName = manager.GetProperty("Email").StringValue,
                                Email = manager.GetProperty("Email").StringValue,
                                LastName = manager.GetProperty("LastName").StringValue,
                                FirstName = manager.GetProperty("FirstName").StringValue,
                                LastIpAddress = "192.168.225.1",
                                IsEmailUnsubscribed = false,
                                IsPhoneNumberUnsubscribed = false,
                                LastLoginDateUtc = DateTime.UtcNow,
                                CreatedOnUtc = DateTime.UtcNow,
                                PhoneNumber = manager.GetProperty("PhoneNumber").StringValue,
                                TwoFactorEnabled = false
                            };

                            if (isNew)
                            {
                                var intialPassword =
                                    UppercaseFirstLetter(manager.GetProperty("FirstName").StringValue.ToLower()) + "@" +
                                    manager.GetProperty("PhoneNumber").StringValue;
                                var result = await UserManager.CreateAsync(user, intialPassword);
                                if (result.Succeeded)
                                {
                                    await this.UserManager.AddToRoleAsync(user.Id, "Doctor");
                                    _importManager.ImportDoctorsFromXlsx(file.InputStream, user.Id, iRow);
                                    SuccessNotification("Doctor's records imported successfully !");
                                }
                            }
                            else
                            {
                                ErrorNotification("Record already avilable !!");
                                //update
                            }

                            iRow++;
                        }
                    }
                }
                else
                {
                    ErrorNotification("File format not correct or empty !");
                }
                return RedirectToAction("ImportDoctor");

            }
            catch (Exception exc)
            {
                ErrorNotification("something went wrong !");
                return RedirectToAction("ImportDoctor");
            }
        }

        #endregion
    }
}