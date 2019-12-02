using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSignalR.Components
{
    /// <summary>聊天客户端连接信息</summary>
    public class ChatClient
    {
        public string UserName { get; set; }
        public string ConnectionID { get; set; }

        /// <summary>所有用户</summary>
        public static List<ChatClient> All = new List<ChatClient>();

        /// <summary>获取连接用户</summary>
        public static ChatClient Get(string connectionId)
        {
            return All.FirstOrDefault(t => t.ConnectionID == connectionId);
        }

        /// <summary>连接</summary>
        public static ChatClient Connect(string userName, string connectionId)
        {
            var conn = All.FirstOrDefault(t => t.UserName == userName);
            if (conn == null)
            {
                conn = new ChatClient() { UserName = userName };
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

}