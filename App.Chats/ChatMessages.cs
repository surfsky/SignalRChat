using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Chats
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
        TalkToAll,

        //
        Timer
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
        public object Data { get; set; }

        //
        public string FromID { get; set; }
        public string FromName { get; set; }
        public string ToID { get; set; }
        public string ToName { get; set; }

        //
        public MessageReply() { }

        /// <summary>构造方法</summary>
        public MessageReply(MessageType type, object data, ChatClient from, ChatClient to)
        {
            this.Type = type;
            this.Result = true;
            this.Date = DateTime.Now;
            this.Data = data;

            this.FromID = from?.ConnectionID;
            this.FromName = from?.UserName;
            this.ToID = to?.ConnectionID;
            this.ToName = to?.UserName;
        }
    }


}