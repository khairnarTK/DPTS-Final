using DPTS.Domain.Core.Address;
using DPTS.Domain.Core.Country;
using DPTS.Domain.Core.Doctors;
using DPTS.Domain.Core.Notification;
using DPTS.Domain.Core.Speciality;
using DPTS.Domain.Core.StateProvince;
using DPTS.Domain.Core.SubSpeciality;
using DPTS.Domain.Address;
using DPTS.Domain.Core.ExportImport;
using DPTS.Domain.Country;
using DPTS.Domain.Notification;
using DPTS.Domain.Speciality;
using DPTS.Domain.StateProvince;
using DPTS.Domain.SubSpeciality;
using DPTS.Services.Doctors;
using DPTS.Services.ExportImport;
using DPTS.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof (NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof (NinjectWebCommon), "Stop")]

namespace DPTS.Web
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Domain.Core;
    using Domain;
    using Domain.Core.Appointment;
    using Domain.Appointment;
    using EmailSmsNotifications.IServices;
    using EmailSmsNotifications.Services;
    using Domain.Core.ReviewComments;
    using Domain.ReviewComments;
    using Domain.Common;
    using Services.Common;
    using Microsoft.AspNet.Identity;
    using Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Domain.Core.Common;
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof (OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof (NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(typeof(IRepository<>)).To(typeof (Repository<>)).InRequestScope();
            kernel.Bind<IDoctorService>().To<DoctorService>();
            kernel.Bind<ISpecialityService>().To<SpecialityService>();
            kernel.Bind<ICountryService>().To<CountryService>();
            kernel.Bind<IStateProvinceService>().To<StateProvinceService>();
            kernel.Bind<ISubSpecialityService>().To<SubSpecialityService>();
            kernel.Bind<IAddressService>().To<AddressService>();
            kernel.Bind<IEmailCategoryService>().To<EmailCategoryService>();
            kernel.Bind<IAppointmentService>().To<AppointmentService>();
            kernel.Bind<IDefaultNotificationSettingsService>().To<DefaultNotificationSettingsService>();
            kernel.Bind<ISmsNotificationService>().To<SmsNotificationService>();
            kernel.Bind<IEmailNotificationService>().To<EmailNotificationService>();
            kernel.Bind<IImportManager>().To<ImportManager>();
            kernel.Bind<IReviewCommentsService>().To<ReviewCommentsService>();
            kernel.Bind<IPictureService>().To<PictureService>();
            kernel.Bind<IQualifiactionService>().To<QualifiactionService>();
            //kernel.Bind<ApplicationSignInManager>().To<ApplicationSignInManager>();
            //kernel.Bind<IUserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>();
            //kernel.Bind<UserManager<ApplicationUser>>().ToSelf();
            //kernel.Bind<IAuthenticationManager>().ToMethod(c =>
            //        HttpContext.Current.GetOwinContext().Authentication).InRequestScope();

        }
    }
}