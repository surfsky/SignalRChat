using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Hubs
{
    /// <summary>
    /// 简单的 SignalR 聊天 Hub
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>广播消息（谁发了啥）</summary>
        public async Task Broadcast(string user, string message)
        {
            // 给所有客户端发消息，附带2个参数
            await Clients.All.SendAsync("showMessage", user, message);
        }
    }
}
