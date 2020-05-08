using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace App.Hubs
{
    /// <summary>
    /// 聊天 Hub. SignalR 好像只留下这个模式了。
    /// </summary>
    [Authorize]
    public class IMHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var name = Context.User.Identity.Name;
            var msg = new ServerMessage(MessageType.SignIn, null, null, "登录成功");
            return Send(connectionId, msg);
        }

        /// <summary>消息处理</summary>
        //protected Task Do(MessageType type, string to, string data)
        protected Task Do(string data)
        {
            // 解析data
            ClientMessage msg = data.ParseJson<ClientMessage>(true);
            if (msg == null)
                return Broadcast(null);

            //
            var connectionId = Context.ConnectionId;
            var from = IMClient.Get(connectionId);
            var to = msg.To.IsEmpty() ? null : IMClient.Get(msg.To);
            var userId = Context.User.Claims.FirstOrDefault(t => t.Type == "UserID").Value;
            var userName = Context.User.Claims.FirstOrDefault(t => t.Type == "UserName").Value;

            // 注册操作（将用户信息和ConnectionID联系起来）
            if (msg.Type == MessageType.SignIn)
            {
                //var o = msg.Data.ParseDynamic();
                //var userId = o.UserId;
                //var userName = o.UserName;
                from = IMClient.Connect(connectionId, userId, userName);
                var d = new ServerMessage(msg.Type, from, to, "");
                return Send(connectionId, d);
            }
            if (msg.Type == MessageType.SignOut)
            {
                IMClient.Disconnect(connectionId);
                var d = new ServerMessage(msg.Type, from, to, "");
                return Send(connectionId, d);
            }

            // 组操作
            if (msg.Type == MessageType.JoinGroup)
            {   
                Groups.AddToGroupAsync(connectionId, msg.To);
                var d = new ServerMessage(msg.Type, from, to, $"{from?.NickName} 加入组 {msg.To}");
                return SendToGroup(connectionId, d);
            }
            if (msg.Type == MessageType.QuitGroup)
            {
                Groups.RemoveFromGroupAsync(connectionId, msg.To);
                var d = new ServerMessage(msg.Type, from, to, $"{from?.NickName} 退出组 {msg.To}");
                return Send(connectionId, d);
            }

            // 聊天
            if (msg.Type == MessageType.TalkToOne)
            {
                var ids = new List<string>() { msg.To };
                if (from?.ConnectionID != to?.ConnectionID)
                    ids.Add(connectionId);
                var d = new ServerMessage(msg.Type, from, to, msg.Data);
                return Send(ids, d);
            }
            if (msg.Type == MessageType.TalkToAll)
            {
                var d = new ServerMessage(msg.Type, from, to, msg.Data);
                return Broadcast(d);
            }
            if (msg.Type == MessageType.TalkToGroup)
            {
                var d = new ServerMessage(msg.Type, from, to, msg.Data);
                return SendToGroup(msg.To, d);
            }

            //
            var m = new ServerMessage(MessageType.Others, from, to, data);
            return Broadcast(m);
        }


        //----------------------------------------------
        // 消息
        //----------------------------------------------
        /// <summary>发送给指定用户</summary>
        private Task Send(string connectionId, ServerMessage msg)
        {
            return Clients.Client(connectionId).SendAsync("chat", msg.ToJson());
        }

        /// <summary>发送给几个用户</summary>
        private Task Send(List<string> ids, ServerMessage d)
        {
            return Clients.Clients(ids).SendAsync("chat", d.ToJson());
        }

        /// <summary>发送给组</summary>
        private Task SendToGroup(string group, ServerMessage d)
        {
            return Clients.Group(group).SendAsync("chat", d.ToJson());
        }

        /// <summary>广播</summary>
        private Task Broadcast(ServerMessage msg)
        {
            return Clients.All.SendAsync("chat", msg.ToJson());
        }
    }
}