using System;
using System.Threading.Tasks;
using Microsoft.Owin;        
using Microsoft.Owin.Cors;
using Owin;
using App.Chats;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(TestSignalR.App_Start.Startup))]
namespace TestSignalR.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);   // 允许跨域访问，必须先安装包 Microsoft.Owin.Cors
            app.MapSignalR<ChatConnection>("/Chats");
            //app.MapSignalR("/signalr", new HubConfiguration());
            app.MapSignalR();
            //var config = new HubConfiguration()
            //{
            //    EnableDetailedErrors = true
            //};
            //app.MapSignalR(config);
            //app.MapSignalR();  //
            //app.MapHubs();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHub<ChatHub>("/Chat");
            //});

            //
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //hubContext.Clients.All...
        }
    }
}
