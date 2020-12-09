using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using App.Core;
using Microsoft.AspNet.SignalR.Hubs;

namespace App.Chats
{
    /// <summary>
    /// Hub 的方式实在不能接受。
    /// 客户端调用服务器端方法，服务器端调用客户端方法，没必要这么灵活，很难维护和理解。
    /// </summary>
    //[HubName("ChatHub")]
    public class ChatHub : Hub
    {
        /// <summary>客户端连接的时候调用</summary>
        public override Task OnConnected()
        {
            Trace.WriteLine("客户端连接成功");
            return base.OnConnected();
        }

        /// <summary>广播</summary>
        public void Broadcast(string message)
        {
            // 调用客户端 方法（Clients.All 是一个 dynamic 对象）。net core 版本该方式已经被放弃。
            //Clients.All.showMessage(message);
            // 或用该方法
            (Clients.All as IClientProxy).Invoke("showMessage", message);
        }

    }
}