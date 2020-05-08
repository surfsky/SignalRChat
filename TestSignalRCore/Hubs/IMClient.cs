using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Hubs
{
    /// <summary>聊天客户信息</summary>
    public class IMClient
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string ConnectionID { get; set; }

        /// <summary>所有用户</summary>
        public static List<IMClient> All = new List<IMClient>();

        /// <summary>获取连接用户</summary>
        public static IMClient Get(string connectionId)
        {
            return All.FirstOrDefault(t => t.ConnectionID == connectionId);
        }

        /// <summary>连接</summary>
        public static IMClient Connect(string connectionId, string id, string nickName)
        {
            var conn = All.FirstOrDefault(t => t.NickName == nickName);
            if (conn == null)
            {
                conn = new IMClient() {Id = id, NickName = nickName };
                All.Add(conn);
            }
            conn.ConnectionID = connectionId;
            return conn;
        }

        /// <summary>断开</summary>
        public static void Disconnect(string connectionId)
        {
            var conn = All.FirstOrDefault(t => t.ConnectionID == connectionId);
            if (conn != null)
                All.Remove(conn);
        }
    }

}