using System.Web;
using System.Web.Mvc;
using WebSite.Filters;

namespace WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new DisposeFilterAttribute());
        }
    }
}