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
    /// <summary>SignalR 聊天服务器</summary>
    public class ChatConnection : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var name = request.User?.Identity?.Name;
            return base.OnConnected(request, connectionId);
        }

        /// <summary>消息处理</summary>
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            // 解析data
            Message msg = null;
            try 
            {
                msg = data.ParseJson<Message>(); 
            }
            catch 
            {
                return Connection.Broadcast(data);
            }

            var from = ChatClient.Get(connectionId);
            var to = msg.To.IsEmpty() ? null : ChatClient.Get(msg.To);

            // 注册操作（将用户信息和ConnectionID联系起来）
            if (msg.Type == MessageType.Connect)
            {
                from = ChatClient.Connect(msg.Name, connectionId);
                return Connection.Send(connectionId, Reply(MessageType.Connect, from, to, $"{connectionId}"));
            }
            if (msg.Type == MessageType.Disconnect)
            {
                ChatClient.Disconnect(msg.Name);
                return Connection.Send(connectionId, Reply(MessageType.Disconnect, from, to, ""));
            }

            // 组操作
            if (msg.Type == MessageType.JoinGroup)
            {
                Groups.Add(connectionId, msg.To);
                return Connection.Send(connectionId, Reply(MessageType.JoinGroup, from, to, $"{from?.UserName} 加入组 {msg.To}"));
            }
            if (msg.Type == MessageType.QuitGroup)
            {
                Groups.Remove(connectionId, msg.To);
                return Connection.Send(connectionId, Reply(MessageType.QuitGroup, from, to, $"{from?.UserName} 退出组 {msg.To}"));
            }

            // 聊天
            if (msg.Type == MessageType.TalkToOne)
            {
                var ids = new List<string>() { msg.To };
                if (from?.ConnectionID != to?.ConnectionID)
                    ids.Add(connectionId);
                return Connection.Send(ids, Reply(MessageType.TalkToOne, from, to, msg.Data));
            }
            if (msg.Type == MessageType.TalkToAll)       
                return Connection.Broadcast(Reply(MessageType.TalkToAll, from, to, msg.Data));
            if (msg.Type == MessageType.TalkToGroup)     
                return Groups.Send(msg.To, Reply(MessageType.TalkToGroup, from, to, msg.Data));

            //
            return Connection.Broadcast(data);
        }

        /// <summary>包裹发送到客户端的信息</summary>
        public string Reply(MessageType type, ChatClient from, ChatClient to, object data)
        {
            return new MessageReply(type, data, from, to).ToJson();
        }
    }

}