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

        // ÿ�������Ӹ� SignalR �ͻ��˷���ϵͳʱ����Ϣ
        // �򵥡�����ʱ�Ķ�ʱ���ȣ��磺�����档��վ������ʱ��ʱ���ɱ����
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
            t.AutoReset = true; // ѭ��ִ��(true)��
            t.Enabled = true;
        }
    }
}
