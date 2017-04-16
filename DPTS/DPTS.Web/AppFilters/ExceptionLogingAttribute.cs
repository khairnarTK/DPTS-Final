using System.Web.Mvc;
using DPTS.Logging;
using DPTS.Logging.AppInfra;
using DPTS.Logging.Contracts;
using DPTS.Logging.Models;

namespace DPTS.Web.AppFilters
{
    public class ExceptionLogingAttribute : IExceptionFilter
    {
        private readonly ILogManager _fileLogger;

        public ExceptionLogingAttribute()
        {
            _fileLogger = new FileLogManager();
        }

        public void OnException(ExceptionContext filterContext)
        {
            _fileLogger.LogApplicationCalls(GetExceptionEntry(filterContext));
        }

        private static ExceptionEntry GetExceptionEntry(ExceptionContext filterContext)
        {
            return new ExceptionEntry
            {
                Title = Constants.ApplicationName,
                Message = filterContext?.Exception?.Message,
                Exception = filterContext?.Exception?.ToString(),
                StackStrace = filterContext?.Exception?.StackTrace
            };
        }
    }
}