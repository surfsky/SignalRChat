using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using App.Chats;

[assembly: OwinStartup(typeof(TestSignalR.App_Start.Startup))]
namespace TestSignalR.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);   // 允许跨域访问，必须先安装包 Microsoft.Owin.Cors
            app.MapSignalR<ChatConnection>("/Chat");
        }
    }
}
