using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult BadRequest()
        {
            var error = new ErrorData
            {
                ErrorCode = "400",
                ErrorHeading = "Bad Request",
                ErrorContent = "Your browser sent a request that this server could not understand."
            };
            SetViewErrorData(error);
            return View("Error");
        }

        public ActionResult ServerError()
        {
            var error = new ErrorData
            {
                ErrorCode = "500",
                ErrorHeading = "Internal Server Error",
                ErrorContent = "Your browser sent a request that this server could not understand."
            };
            SetViewErrorData(error);
            return View("Error");
        }

        public ActionResult PageNotFound()
        {
            var error = new ErrorData
            {
                ErrorCode = "404",
                ErrorHeading = "Page Not Found",
                ErrorContent =
                    "The page you are looking for might have been removed, had its name changed, or its temporarily unavailable."
            };
            SetViewErrorData(error);
            return View("Error");
        }

        private void SetViewErrorData(ErrorData error)
        {
            ViewBag.ErrorCode = error.ErrorCode;
            ViewBag.ErrorHeading = error.ErrorHeading;
            ViewBag.ErrorContent = error.ErrorContent;
        }
    }

    public class ErrorData
    {
        public string ErrorCode { get; set; }
        public string ErrorHeading { get; set; }
        public string ErrorContent { get; set; }
    }
}