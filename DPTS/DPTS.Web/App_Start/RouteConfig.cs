using System.Web.Mvc;
using System.Web.Routing;

namespace DPTS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new {controller = "Home", action = "Index"}
                );

            routes.MapRoute("HomePage",
                            "",
                            new { controller = "Home", action = "Index" },
                            new[] { "DPTS.Web.Controllers" });

            //get state list by country ID  (AJAX link)
            routes.MapRoute("GetStatesByCountryId",
                "Doctor/GetStatesByCountryId/",
                new {controller = "Doctor", action = "GetStatesByCountryId"}
                );
            routes.MapRoute("Admin",
             "Administration/Index",
             new { controller = "Administration", action = "Index" }
             );
            routes.MapRoute("GetSubSpecialityBySpeciality",
               "SubSpeciality/GetsubSpecialityBySpec/",
               new { controller = "SubSpeciality", action = "GetsubSpecialityBySpec" }
               );
            routes.MapRoute("DPTSSearch",
                "search/",
                new {controller = "Home", action = "Search"});
            routes.MapRoute(
                "BookAppoinment",
                "appoinment/{doctorId}",
                new {controller = "Appointment", action = "AppointmentSchedule", doctorId = UrlParameter.Optional}
                );

            //reviews
            routes.MapRoute("DoctorReviews",
                            "doctorreviews/{doctorId}",
                            new { controller = "Doctor", action = "DoctorReviews", doctorId = UrlParameter.Optional },
                            new[] { "DPTS.Web.Controllers" });
            routes.MapRoute("PatientProductReviews",
                            "patient/doctorreviews",
                            new { controller = "Doctor", action = "PatientDoctorReviews" },
                            new[] { "DPTS.Web.Controllers" });
            routes.MapRoute("PatientDoctorReviewsPaged",
                            "patient/doctorreviews/page/{page}",
                            new { controller = "Doctor", action = "PatientDoctorReviews" },
                            new { page = @"\d+" },
                            new[] { "DPTS.Web.Controllers" });

            routes.MapRoute("SetDoctorReviewHelpfulness",
                          "setdoctorreviewhelpfulness",
                          new { controller = "Doctor", action = "SetDoctorReviewHelpfulness" });

            //blog
            routes.MapRoute("BlogByTag",
                            "blog/tag/{tag}",
                            new { controller = "Blog", action = "BlogByTag" });

            routes.MapRoute("BlogByMonth",
                            "blog/month/{month}",
                            new { controller = "Blog", action = "BlogByMonth" });

            //routes.MapRoute(
            //    "ContactUs",
            //    "contact",
            //    new { controller = "Home", action = "Contact" }
            //);
            //routes.MapRoute(
            //    "AboutUs",
            //    "about",
            //    new { controller = "Home", action = "About" }
            //);
            //routes.MapRoute(
            //    "AdminDashbrd",
            //    "admin",
            //    new { controller = "Administration", action = "Index" }
            //);
            //routes.MapRoute(
            //    "Register",
            //    "register",
            //    new { controller = "Account", action = "Register" }
            //);
            //routes.MapRoute(
            //    "Login",
            //    "login",
            //    new { controller = "Account", action = "Login" }
            //);
            //routes.MapRoute(
            //    "AccountManage",
            //    "manage",
            //    new { controller = "Manage", action = "index" }
            //);
        }
    }
}