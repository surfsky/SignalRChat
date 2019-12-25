using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace TestSignalRWebForm
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);


            //HttpConfiguration cfg = new HttpConfiguration(routes);
            //cfg..EnableCors(new EnableCorsAttribute("*", "*", "*"));
        }
    }
}
