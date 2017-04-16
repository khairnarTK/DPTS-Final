using DPTS.Web.AppInfra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    public class BaseController : Controller
    {
        //private ISmsNotificationService _smsService;
        //public BaseController(ISmsNotificationService smsService)
        //{
        //    _smsService = smsService;
        //}
        public class RolesAttribute : AuthorizeAttribute
        {
            public RolesAttribute(params string[] roles)
            {
                Roles = string.Join(",", roles);
            }
        }
        [HttpPost]
        public bool ClearSession()
        {
            try
            {
                Session.Clear();
                return true;
            }
            catch (Exception e)
            {
                // ExceptionHandler.HandleException(e);
                return false;
            }
        }
        [NonAction]
        protected bool IsValidateId(int id)
        {
            return id != 0;
        }
       
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
               // LogException(exception);

            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("dpts.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

    }
    public class CacheFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the cache duration in seconds. The default is 10 seconds.
        /// </summary>
        /// <value>The cache duration in seconds.</value>
        private int Duration { get; set; }

        public CacheFilterAttribute()
        {
            Duration = 10;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Duration <= 0) return;

            var cache = filterContext.HttpContext.Response.Cache;
            var cacheDuration = TimeSpan.FromSeconds(Duration);

            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.Add(cacheDuration));
            cache.SetMaxAge(cacheDuration);
            cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        }
    }

    public class CompressFilter : ActionFilterAttribute
    {
        //FilterExecutingContext
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var request = filterContext.HttpContext.Request;

        //    var acceptEncoding = request.Headers["Accept-Encoding"];

        //    if (string.IsNullOrEmpty(acceptEncoding)) return;

        //    acceptEncoding = acceptEncoding.ToUpperInvariant();

        //    var response = filterContext.HttpContext.Response;

        //    if (acceptEncoding.Contains("GZIP"))
        //    {
        //        response.AppendHeader("Content-encoding", "gzip");
        //        response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
        //    }
        //    else if (acceptEncoding.Contains("DEFLATE"))
        //    {
        //        response.AppendHeader("Content-encoding", "deflate");
        //        response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
        //    }
        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    filterContext.Controller.ViewBag.MinificationEnabled =
        //        ConfigurationManager.AppSettings["MinificationEnabled"] == "true" ? ".min" : "";
        //}
    }
    
}