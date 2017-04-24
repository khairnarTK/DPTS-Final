using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DPTS.Web.Models;
using System.Collections.Generic;
using DPTS.Domain.Core.Doctors;
using DPTS.Domain.Entities;
using DPTS.EmailSmsNotifications.ServiceModels;
using DPTS.EmailSmsNotifications.IServices;
using reCaptcha;
using System.Configuration;
using System.Net;
using DPTS.Domain.Core.SubSpeciality;
using DPTS.Domain.Core.Speciality;
using DPTS.Data.Context;
using DPTS.Domain.Core.Country;
using DPTS.Domain.Core.StateProvince;
using System.Xml.Linq;
using DPTS.Domain.Core.Address;
using DPTS.Domain.Core.Common;
using DPTS.Domain.Common;

namespace DPTS.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext context;
        private IDoctorService _doctorService;
        private ISmsNotificationService _smsService;
        private readonly ISubSpecialityService _subSpecialityService;
        private readonly ISpecialityService _specialityService;
        private readonly DPTSDbContext _context;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IAddressService _addressService;
        private readonly IQualifiactionService _qualifiactionService;
        private readonly IPictureService _pictureService;

        public AccountController(IDoctorService doctorService,
            ISmsNotificationService smsService,
            ISpecialityService specialityService,
            ISubSpecialityService subSpecialityService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IAddressService addressService,
            IQualifiactionService qualifiactionService,
            IPictureService pictureService)
        {
            context = new ApplicationDbContext();
            _doctorService = doctorService;
            _smsService = smsService;
            _subSpecialityService = subSpecialityService;
            _specialityService = specialityService;
            _context = new DPTSDbContext();
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _addressService = addressService;
            _qualifiactionService = qualifiactionService;
            _pictureService = pictureService;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Recaptcha = ReCaptcha.GetHtml(ConfigurationManager.AppSettings["ReCaptcha:SiteKey"]);
            ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];
            return View();
        }

        [AllowAnonymous]
        public ActionResult Unsubscribe(string id)
        {
            try
            {
                var user = UserManager.FindByIdAsync(id);
                if (user == null || !UserManager.IsEmailConfirmedAsync(id).Result)
                {
                    //TODO Update Values in table
                    ViewBag.UserId = id;
                    ViewBag.Message = "Subscribe Successfully";
                }
            }
            catch (Exception)
            {
                ViewBag.UserId = id;
                ViewBag.Message = "Subscribe unsuccessfully";
                throw;
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Subscribe(string id)
        {
            try
            {
                var user = UserManager.FindByIdAsync(id);
                if (user == null || !UserManager.IsEmailConfirmedAsync(id).Result)
                {
                    //TODO Update Values in table
                    ViewBag.UserId = id;
                    ViewBag.Message = "Subscribe Successfully";
                }
            }
            catch (Exception)
            {
                ViewBag.UserId = id;
                ViewBag.Message = "Subscribe unsuccessfully";
                throw;
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult TermsConditions()
        {
            return View();
        }
        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult> TermsConditions(string Name,string Email,string Message,string Subject,string MailTo)
        //{
        //    try
        //    {
        //        EmailSmsNotifications.Services.EmailNotificationService objE = new EmailSmsNotifications.Services.EmailNotificationService();
        //        EmailSmsNotifications.ServiceModels.EmailNotificationModel model = new EmailNotificationModel
        //        {
        //            content = Message,
        //            from = MailTo,
        //            subject = Subject,
        //            to = Email
        //        };
        //        await objE.SendEmail(model);
        //    }catch(Exception ex) { }
        //    return View();
        //}

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
               if (ModelState.IsValid && ReCaptcha.Validate(ConfigurationManager.AppSettings["ReCaptcha:SecretKey"]))
               {
                if (!ModelState.IsValid)
                {
                    ViewBag.RecaptchaLastErrors = ReCaptcha.GetLastErrors(this.HttpContext);
                    ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];

                    ViewBag.ReturnUrl = returnUrl;
                    return View(model);
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result =
                    await
                        SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, model.RememberMe});
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        ViewBag.RecaptchaLastErrors = ReCaptcha.GetLastErrors(this.HttpContext);
                        ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];
                        ViewBag.ReturnUrl = returnUrl;
                        return View(model);
                }

                }

                ViewBag.RecaptchaLastErrors = ReCaptcha.GetLastErrors(this.HttpContext);

                ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];
                return View(model);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        #region ContactForm
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DoctorContact(ContactUsViewModel model)
        {
            try
            {
                var user = await UserManager.FindByNameAsync(model.DocEmail);
                if (ModelState.IsValid)
                {
                    await UserManager.SendEmailAsync(user.Id, model.Subject, model.Message);
                    SuccessNotification("Email send successfully. we will contact you soon.");
                }
                return RedirectToAction("DoctorDetails", "Doctor", new { doctorId = user.Id });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            // You can configure the account lockout settings in IdentityConfig
            var result =
                await
                    SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe,
                        model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        public IList<SelectListItem> GetUserTypeList()
        {
            List<SelectListItem> typelst = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                }
            };
            typelst.AddRange(context.Roles.ToList().Select(type => new SelectListItem
            {
                Text = type.Name,
                Value = type.Name
            }));
            return typelst;
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            var model = new RegisterViewModel {UserRoleList = GetUserTypeList()};
            ViewBag.Recaptcha = ReCaptcha.GetHtml(ConfigurationManager.AppSettings["ReCaptcha:SiteKey"]);
            ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        [NonAction]
        protected bool IsPhoneNumbersExits(string phoneNumber)
        {
            try
            {
                var phoneNum = _context.AspNetUsers.Where(p => p.PhoneNumber == phoneNumber).FirstOrDefault().PhoneNumber;
                if (phoneNum == null)
                    return true;
            }
            catch { return true; }
            return false;
        }
        [NonAction]
        protected bool IsEmailExits(string eMail)
        {
            try
            {
                var email = _context.AspNetUsers.Where(p => p.Email == eMail).FirstOrDefault().Email;
                if (email == null)
                    return true;
            }
            catch { return true; }
            return false;
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid && ReCaptcha.Validate(ConfigurationManager.AppSettings["ReCaptcha:SecretKey"]))
                {
                    if (IsPhoneNumbersExits(model.PhoneNumber) && IsEmailExits(model.Email))
                    {
                        if (model.Role == "0" && model.UserType == "professional")
                            ModelState.AddModelError("", "Select user type");

                        if (ModelState.IsValid)
                        {
                            SendOtp(model.PhoneNumber);
                            TempData["regmodel"] = model;
                            return RedirectToAction("ConfirmRegistration", "Account");
                        }
                    }
                    else
                    {
                        if(!IsPhoneNumbersExits(model.PhoneNumber))
                            ErrorNotification("Phone number already exists.");

                        if (!IsEmailExits(model.Email))
                            ErrorNotification("Email already exists.");
                    }
                }
                var capErr = ReCaptcha.GetLastErrors(this.HttpContext);
                if (capErr != null)
                    ErrorNotification("Oops!! Invalid Captcha.");

                ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];
                model.UserRoleList = GetUserTypeList();
                return View(model);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        private void SendOtp(string phoneNumber)
        {
            var sms = new SmsNotificationModel
            {
                numbers = phoneNumber,
                route = 4,
                //route 4 is for transactional sms
                senderId = "DOCPTS"
            };
            Session["otp"] = _smsService.GenerateOTP();
            sms.message = "Doctor365 verification code: " + Session["otp"] + "." +
                          "Pls do not share with anyone. It is valid for 10 minutes.";
           // _smsService.SendSms(sms);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmRegistration()
        {
            RegisterViewModel regModel = (RegisterViewModel) TempData["regmodel"];
            ConfirmRegisterViewModel model = new ConfirmRegisterViewModel
            {
                RegistrationDetails = regModel,
                ConfirmOtp = Session["otp"].ToString()
            };
            //if u want to otp then comment follws line
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmRegistration(ConfirmRegisterViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ConfirmOtp != Session["otp"].ToString())
                    {
                        ErrorNotification("Invalid OTP !! Enter correct otp.");
                        // ViewBag.confirmFail = "Invalid OTP!!";
                        return View(model);
                    }

                    var user = new ApplicationUser
                    {
                        UserName = model.RegistrationDetails.Email,
                        Email = model.RegistrationDetails.Email,
                        LastName = model.RegistrationDetails.LastName,
                        FirstName = model.RegistrationDetails.FirstName,
                        LastIpAddress = "192.168.225.1",
                        IsEmailUnsubscribed = false,
                        IsPhoneNumberUnsubscribed = true,
                        LastLoginDateUtc = DateTime.UtcNow,
                        CreatedOnUtc = DateTime.UtcNow,
                        PhoneNumber = model.RegistrationDetails.PhoneNumber,
                        TwoFactorEnabled = true
                    };
                    var result = await UserManager.CreateAsync(user, model.RegistrationDetails.Password);
                    if (result.Succeeded)   
                    {
                        if (model.RegistrationDetails.UserType.ToLowerInvariant() == "professional")
                        {
                            await this.UserManager.AddToRoleAsync(user.Id, model.RegistrationDetails.Role);
                            var doctor = new Doctor { DoctorId = user.Id, RegistrationNumber = model.RegistrationNumber };
                            _doctorService.AddDoctor(doctor);
                        }
                        //gives content to sending thanks email
                        await UserManager.SendEmailAsync(user.Id, "Thank you for registering at Doctor 365", "Thank you!!");
                        await SignInManager.SignInAsync(user, false, false);

                        return RedirectToAction("Index", "Home");
                    }
                    string errorNotify = string.Empty;
                    foreach (var item in result.Errors)
                    {
                        errorNotify += item + " ,";
                    }
                    if (!string.IsNullOrWhiteSpace(errorNotify))
                        ErrorNotification(errorNotify.TrimEnd(','));

                    ViewBag.ReturnUrl = returnUrl;
                    AddErrors(result);
                }
            }
            catch { }
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]  
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)//|| !await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                 var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                 await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                 return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose}).ToList();
            return
                View(new SendCodeViewModel {Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode",
                new {Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe});
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false, false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        public void AddPicture(int pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute,
            string doctorId)
        {
            try
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
            }
            catch { }
        }

        [AllowAnonymous]
        public ActionResult JoinUs()
        {
            try
            {
                var model = new JoinUsViewModel();
                List<SelectListItem> typelst = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Select",
                        Value = "0"
                    }
                };
                //  model.SpecialityList = GetSpecialityList();
                //model.SubSpecialityList = typelst;
                model.AddressModel.AvailableCountry = GetCountryList();
                model.AddressModel.AvailableStateProvince = typelst;
                model.AvilableQualification = GetAvilableQualifications();
                ViewBag.GenderList = GetGender();
                return View(model);
            }
            catch { throw; }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> JoinUs(JoinUsViewModel model)
        {
            // string joined = string.Join(",", model.Expertise);
            try
            {
                //if (model.AddressModel.CountryId == 0 || model.AddressModel.StateProvinceId == 0)
                //{
                //    ModelState.AddModelError("Select Country or State !!", "");
                //    ErrorNotification("Select Country or State");
                //}
                //if (!model.Expertise.Any())
                //{
                //    ModelState.AddModelError("select at least one qualification !!", "");
                //    ErrorNotification("select at least one qualification !!");
                //}
                if(model.PictureId == 0)
                {
                    ModelState.AddModelError("upload picture !!", "");
                    ErrorNotification("upload picture !!");
                }
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        LastName = model.LastName,
                        FirstName = model.FirstName,
                        LastIpAddress = "192.168.225.1",
                        IsEmailUnsubscribed = false,
                        IsPhoneNumberUnsubscribed = false,
                        LastLoginDateUtc = DateTime.UtcNow,
                        CreatedOnUtc = DateTime.UtcNow,
                        PhoneNumber = model.PhoneNumber,
                        TwoFactorEnabled = false
                    };
                    var result = await UserManager.CreateAsync(user, "Test@123");
                    if (result.Succeeded)
                    {
                        var dateOfBirth = model.ParseDateOfBirth();
                        await this.UserManager.AddToRoleAsync(user.Id, "Doctor");
                        var doctor = new Doctor
                        {
                            DoctorId = user.Id,
                            RegistrationNumber = model.RegistrationNumber,
                            Gender = model.Gender,
                            DateOfBirth = dateOfBirth.ToString(),
                            Expertise = (model.Expertise.Any()) ? string.Join(",", model.Expertise) : string.Empty,
                            MiddleName = model.MiddleName,
                            Subscription=model.Subscription
                        };
                        _doctorService.AddDoctor(doctor);
                        //if (!string.IsNullOrWhiteSpace(doctor.DoctorId) && model.Speciality > 0 && model.SubSpeciality > 0)
                        //{
                        //    var specMap = new SpecialityMapping
                        //    {
                        //        Doctor_Id = doctor.DoctorId,
                        //        Speciality_Id = model.Speciality,
                        //        SubSpeciality_Id = model.SubSpeciality
                        //    };
                        //    _specialityService.AddSpecialityByDoctor(specMap);
                        //}
                        var address = new Address
                        {
                            StateProvinceId = model.AddressModel.StateProvinceId,
                            CountryId = model.AddressModel.CountryId,
                            Address1 = model.AddressModel.Address1,
                            Address2 = model.AddressModel.Address2,
                            Hospital = model.AddressModel.Hospital,
                            FaxNumber = model.AddressModel.FaxNumber,
                            PhoneNumber = model.AddressModel.LandlineNumber,
                            Website = model.AddressModel.Website,
                            ZipPostalCode = model.AddressModel.ZipPostalCode,
                            City = model.AddressModel.City
                        };

                        if (address.CountryId == 0)
                            address.CountryId = null;
                        if (address.StateProvinceId == 0)
                            address.StateProvinceId = null;

                        string state = address.StateProvinceId == 0
                            ? string.Empty
                            : _stateProvinceService.GetStateProvinceById(model.AddressModel.StateProvinceId).Name;
                        string docAddress = model.AddressModel.Address1 + ", " + model.AddressModel.City + ", " + state + ", " + model.AddressModel.ZipPostalCode;
                        var geoCoodrinate = GetGeoCoordinate(docAddress);
                        if (geoCoodrinate.Count == 2)
                        {
                            address.Latitude = geoCoodrinate[AppInfra.Constants.Lat];
                            address.Longitude = geoCoodrinate[AppInfra.Constants.Lng];
                        }
                        else
                        {
                            var geoCoodrinates = GetGeoCoordinate(model.AddressModel.ZipPostalCode);
                            if (geoCoodrinates.Count == 2)
                            {
                                address.Latitude = geoCoodrinates[AppInfra.Constants.Lat];
                                address.Longitude = geoCoodrinates[AppInfra.Constants.Lng];
                            }
                        }
                        _addressService.AddAddress(address);

                        if(model.PictureId > 0 && !string.IsNullOrWhiteSpace(doctor.DoctorId))
                        {
                            AddPicture(model.PictureId, 0, null, null, doctor.DoctorId);
                        }

                        await UserManager.SendEmailAsync(user.Id, "Thanks for the registration. We'll verify your details.", "For more queries drops us a email on <b>dptsus@outlook.com</b>");
                        return RedirectToAction("ThanksJoinUs", "Account");
                    }
                    if (!string.IsNullOrWhiteSpace(result.Errors.LastOrDefault()))
                        ErrorNotification(result.Errors.LastOrDefault());
                }
                List<SelectListItem> typelst = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Select",
                        Value = "0"
                    }
                };
                model.AddressModel.AvailableCountry = GetCountryList();
               // model.SpecialityList = GetSpecialityList();
                //model.SubSpecialityList = typelst;
                model.AddressModel.AvailableStateProvince = typelst;
                ViewBag.GenderList = GetGender();
                return View(model);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        public IList<string> GetAvilableQualifications()
        {
            List<string> lst = new List<string>();
            try
            {
                var qul = _qualifiactionService.GetAllQualifiaction().Where(m =>m.IsActive);
                foreach (var item in qul)
                {
                    lst.Add(item.Name);
                }
                return lst;
            }
            catch { return lst; }

        }
        public IList<SelectListItem> GetCountryList()
        {
            try
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
            }catch { throw; }
        }
        [AllowAnonymous]
        public ActionResult ThanksJoinUs()
        {
            return View();
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
        
        public IList<SelectListItem> GetSpecialityList()
        {
            var specialitys = _specialityService.GetAllSpeciality(false);

            List<SelectListItem> typelst = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                }
            };
            if (specialitys == null)
                return typelst;

            typelst.AddRange(specialitys.ToList().Select(type => new SelectListItem
            {
                Text = type.Title,
                Value = type.Id.ToString()
            }));
            return typelst;
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

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}