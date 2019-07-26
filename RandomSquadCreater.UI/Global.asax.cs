using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RandomSquadCreater.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {



            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            log4net.Config.XmlConfigurator.Configure();
            log4net.GlobalContext.Properties["nSightsLog.log"] = DateTime.Now.Date.ToString("ddMMyyyy") + "csv";
        }
    }
}
