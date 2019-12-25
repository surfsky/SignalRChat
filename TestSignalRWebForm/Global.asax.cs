using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace TestSignalRWebForm
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        /// <summary>允许跨域（未测试）</summary>
        public static void EnableCros()
        {
            var origin = HttpContext.Current.Request.Headers["Origin"];
            HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", origin);
        }

    }
}