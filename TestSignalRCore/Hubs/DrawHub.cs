using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Hubs
{
    /// <summary>
    /// 绘图 signalR 服务
    /// </summary>
    public class DrawHub : Hub
    {
        public Task Draw(int prevX, int prevY, int currentX, int currentY, string color)
        {
            return Clients.Others.SendAsync("draw", prevX, prevY, currentX, currentY, color);
        }

        public async Task MoveShape(int x, int y)
        {
            await Clients.Others.SendAsync("shapeMoved", x, y);
        }

    }
}
