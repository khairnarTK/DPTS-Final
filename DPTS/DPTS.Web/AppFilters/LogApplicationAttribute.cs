using System;
using System.Diagnostics;
using System.Web.Mvc;
using DPTS.Logging;
using DPTS.Logging.AppInfra;
using DPTS.Logging.Contracts;
using DPTS.Logging.Models;

namespace DPTS.Web.AppFilters
{
    public class LogApplicationAttribute : ActionFilterAttribute
    {
        private Stopwatch _stopwatch;

        private readonly ILogManager _fileLogger;

        public LogApplicationAttribute()
        {
            _fileLogger = new FileLogManager();
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _stopwatch = new Stopwatch();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            var eventEntry = GetEventEntry(actionExecutedContext);
            _fileLogger.LogApplicationCalls(eventEntry);
            // base.OnActionExecuted(actionExecutedContext);
        }

        private EventEntry GetEventEntry(ActionExecutedContext actionExecutedContext)
        {
            return new EventEntry
            {
                Title = Constants.ApplicationName + "-" +
                        actionExecutedContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                CallType = actionExecutedContext.ActionDescriptor.ActionName,
                ResponseTime = Convert.ToDecimal(_stopwatch.Elapsed.TotalSeconds),
                Status = "True",
            };
        }
    }
}