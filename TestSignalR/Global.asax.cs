using App.Core;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TestSignalR.Components;

namespace TestSignalR
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //RouteTable.Routes.MapConnection<ChatConnection>("echo", "echo/{*operation}");

            InitSchedule();
        }

        // 每隔几分钟给 SignalR 客户端发送系统时间消息
        // 简单、不耗时的定时调度，如：清理缓存。网站无请求时随时会给杀掉。
        private static void InitSchedule()
        {
            var minutes = 0.5;
            var t = new System.Timers.Timer(1000 * 60 * minutes);
            t.Elapsed += (s, te) =>
            {
                var msg = new MessageReply(MessageType.Timer, null, null, null);
                var signalContext = GlobalHost.ConnectionManager.GetConnectionContext<ChatConnection>();
                signalContext.Connection.Broadcast(msg.ToJson());
            };
            t.AutoReset = true; // 循环执行(true)；
            t.Enabled = true;
        }
    }
}
