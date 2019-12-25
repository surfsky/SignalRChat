using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using App.Chats;
//using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(TestSignalR.App_Start.Startup))]

namespace TestSignalR.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);
            //var cfg = new Microsoft.AspNet.SignalR.ConnectionConfiguration() { EnableJSONP = true };
            //app.MapSignalR<ChatConnection>("/Chat", cfg);
            app.MapSignalR<ChatConnection>("/Chat");
        }
    }
}
