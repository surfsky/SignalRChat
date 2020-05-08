using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using App.Core;

namespace App.Chats
{
    /// <summary>
    /// Hub 的方式实在不能接受。
    /// 客户端调用服务器端方法，服务器端调用客户端方法，没必要这么灵活，很难维护和理解。
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>客户端连接的时候调用</summary>
        public override Task OnConnected()
        {
            Trace.WriteLine("客户端连接成功");
            return base.OnConnected();
        }

        /// <summary>供客户端调用的服务器端代码</summary>
        public void Send(string message)
        {
            var name = Guid.NewGuid().ToString().ToUpper();
            // 调用所有客户端的sendMessage方法???
            Clients.All.sendMessage(name, message);
        }

    }
}