SignalR

SignalR是一个.NET Core/.NET Framework的开源实时框架. SignalR的可使用Web Socket, Server Sent Events 和 Long Polling作为底层传输方式.
SignalR基于这三种技术构建, 抽象于它们之上, 它让你更好的关注业务问题而不是底层传输技术问题.
SignalR这个框架分服务器端和客户端, 服务器端支持ASP.NET Core 和 ASP.NET; 而客户端除了支持浏览器里的javascript以外, 也支持其它类型的客户端, 例如桌面应用.
SignalR采用RPC范式来进行客户端与服务器端之间的通信.
Hub协议的默认协议是JSON, 还支持另外一个协议是MessagePack. MessagePack是二进制格式的, 它比JSON更紧凑, 而且处理起来更简单快速, 因为它是二进制的.



ps. 
    感觉就是wcf的简化和Web化，且客户端无需关注用的是什么通道
    WindowForm可用 SignalR.Client 包获取支持