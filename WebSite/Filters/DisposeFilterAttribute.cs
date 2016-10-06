using Data.Contexts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Filters
{
    public class DisposeFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var container = UnityConfig.GetConfiguredContainer();
            var context = container.Resolve<DbContext>();
            context.Dispose();
            container.Teardown(context);
            //base.OnResultExecuted(filterContext);
        }
    }
}