using System.Web.Mvc;
using DPTS.Web.AppFilters;

namespace DPTS.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionLogingAttribute());
            filters.Add(new LogApplicationAttribute());
            //filters.Add(new CompressFilter()); 
        }
    }
}