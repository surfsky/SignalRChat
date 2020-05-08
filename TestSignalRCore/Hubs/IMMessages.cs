using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Hubs
{
    /// <summary>消息类别</summary>
    public enum MessageType
    {
        // 进出
        SignIn,
        SignOut,

        // 组操作
        JoinGroup,
        QuitGroup,
        CreateGroup,
        DeleteGroup,
        KickOutGroup,

        // 聊天操作
        TalkToOne,
        TalkToGroup,
        TalkToAll,

        //
        Timer,
        Others
    }

    /// <summary>客户端发送给服务器的消息</summary>
    public class ClientMessage
    {
        /// <summary>消息类别</summary>
        public MessageType Type { get; set; }

        /// <summary>消息目标</summary>
        public string To { get; set; }

        /// <summary>数据（如文本、密码等）</summary>
        public string Data { get; set; }
    }

    /// <summary>服务器发给客户端的消息</summary>
    public class ServerMessage
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
        public ServerMessage() { }

        /// <summary>构造方法</summary>
        public ServerMessage(MessageType type, IMClient from, IMClient to, object data)
        {
            this.Type = type;
            this.Result = true;
            this.Date = DateTime.Now;
            this.Data = data;

            this.FromID = from?.ConnectionID;
            this.FromName = from?.NickName;
            this.ToID = to?.ConnectionID;
            this.ToName = to?.NickName;
        }
    }


}