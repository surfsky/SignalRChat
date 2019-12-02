using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using App.Core;

namespace TestSignalR.Components
{
    /// <summary>消息类别</summary>
    public enum MessageType
    {
        Connect,
        Disconnect,
        JoinGroup,
        QuitGroup,
        TalkToOne,
        TalkToGroup,
        TalkToAll
    }

    /// <summary>发送给服务器的消息</summary>
    public class Message
    {
        /// <summary>消息类别</summary>
        public MessageType Type { get; set; }

        /// <summary>消息目标</summary>
        public string To { get; set; }

        /// <summary>数据（如文本、密码等）</summary>
        public string Data { get; set; }

        /// <summary>用户名</summary>
        public string Name { get; set; }
    }

    /// <summary>服务器回发的消息</summary>
    public class MessageReply
    {
        public MessageType Type { get; set; }
        public bool Result { get; set; }

        public DateTime Date { get; set; }
        public string FromID { get; set; }
        public string ToID { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public object Data { get; set; }
    }


    /// <summary>客户端连接信息</summary>
    public class Client
    {
        public string UserName { get; set; }
        public string Group { get; set; }
        public string ConnectionID { get; set; }

        /// <summary>所有用户</summary>
        public static List<Client> All = new List<Client>();

        /// <summary>获取连接用户</summary>
        public static Client Get(string connectionId)
        {
            return All.FirstOrDefault(t => t.ConnectionID == connectionId);
        }

        /// <summary>连接</summary>
        public static Client Connect(string userName, string connectionId)
        {
            var conn = All.FirstOrDefault(t => t.UserName == userName);
            if (conn == null)
            {
                conn = new Client() { UserName = userName };
                All.Add(conn);
            }
            conn.ConnectionID = connectionId;
            return conn;
        }

        /// <summary>断开</summary>
        public static void Disconnect(string userName)
        {
            var conn = All.FirstOrDefault(t => t.UserName == userName);
            if (conn != null)
                All.Remove(conn);
        }
    }


    /// <summary>SignalR 聊天服务器</summary>
    public class ChatConnection : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var name = request.User?.Identity?.Name;
            return base.OnConnected(request, connectionId);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            // 解析data
            // var info = $"[{DateTime.Now:HH:mm:ss}] {connectionId} say: {data}";
            var msg = data.ParseJson<Message>();
            var from = Client.Get(connectionId);
            var to = msg.To.IsEmpty() ? null : Client.Get(msg.To);

            // 注册操作（将用户信息和ConnectionID联系起来）
            if (msg.Type == MessageType.Connect)
            {
                from = Client.Connect(msg.Name, connectionId);
                return Connection.Send(connectionId, Reply(MessageType.Connect, from, to, $"{connectionId}"));
            }
            if (msg.Type == MessageType.Disconnect)
            {
                Client.Disconnect(msg.Name);
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
                return Connection.Send(ids, Reply(MessageType.TalkToOne, from, to, msg.Data)); // 给别人发一条
            }
            if (msg.Type == MessageType.TalkToAll)       
                return Connection.Broadcast(Reply(MessageType.TalkToAll, from, to, msg.Data));
            if (msg.Type == MessageType.TalkToGroup)     
                return Groups.Send(msg.To, Reply(MessageType.TalkToGroup, from, to, msg.Data));


            // 返回
            return Connection.Broadcast(connectionId, data);
        }

        /// <summary>包裹发送到客户端的信息</summary>
        public string Reply(MessageType type, Client from, Client to, object data)
        {
            var msg = new MessageReply();
            msg.Type = type;
            msg.Result = true;
            msg.Date = DateTime.Now;
            msg.FromID = from?.ConnectionID;
            msg.FromName = from?.UserName;
            msg.ToID = to?.ConnectionID;
            msg.ToName = to?.UserName;
            msg.Data = data;
            return msg.ToJson();
        }
    }

}