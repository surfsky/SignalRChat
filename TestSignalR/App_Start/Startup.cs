using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using App.Chats;

[assembly: OwinStartup(typeof(TestSignalR.App_Start.Startup))]

namespace TestSignalR.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<ChatConnection>("/Chat");
            //app.MapSignalR();
        }
    }
}
