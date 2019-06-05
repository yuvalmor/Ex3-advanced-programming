using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exercise3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           routes.MapRoute("Display", "display/{ip}/{port}/{time}",
           defaults: new { controller = "Flight", action = "Display", time = UrlParameter.Optional });

           routes.MapRoute("Save", "save/{ip}/{port}/{frequency}/{duration}/{fileName}",
           defaults: new { controller = "Flight", action = "Save",});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Flight", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
