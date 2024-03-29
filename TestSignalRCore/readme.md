﻿# About

Asp.NET core SignalR 聊天示例程序


# Author

<http://github.com/surfsky/TestSignalR>

# Chat 功能
- [x] 发送
- [x] 广播
- [x] 接收广播并显示

# 绘图功能
- [x] 拖动同步
- [x] 绘图同步

# IM 功能
- [ ] 登录验证
- [ ] 连接（关联用户名和ConnectionID）
- [ ] 退出
- [ ] 加入组
- [ ] 退出组
- [ ] 发送给发送人
- [ ] 发送给指定人
- [ ] 发送给组
- [ ] 发送给全体
- [ ] 发送文本
- [ ] 服务器端主动给客户端发消息
- [ ] 发送图片： 参考 http://www.cppcns.com/wangluo/aspnet/147133.html， 用base64传递图片


# 截图

- 简单聊天
![](chat1.png)

- 完整聊天
![](chat2.png)



# Reference

- https://dotnet.microsoft.com/apps/aspnet/signalr
- https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/signalr?tabs=visual-studio&view=aspnetcore-3.1
- [Jabbr](https://github.com/JabbR/JabbR) , SignalR 作者写的聊天室程序。


# Remarks

- aspnetcore 官方版本的 signalr 
  
    - 已经剥离了 jquery
    - 已经不支持 PersistentConnection 模式，只能用 hub方式实现
    - 经测试，无法用 jquery 版本的 signalr 来连接。详见 /Pages/Chat1.cshtml


- 可参考 SignalR-samples-master 编写示例
    - Chat                : 简单的聊天示例。客户端代码在 Pages/Index，服务器端代码在 hubs 和 startup 内
    - Move                : 拖动示例
    - WhiteBoard          : 绘画示例
    - WindowsFormsSample  : 聊天示例windows form客户端，服务器地址要填写 http://.../chat
    - PullRequest         : 
    - StockTickR          : 
    - AndroidJavaClient   : 
    - WindowsUniversal    : 
    - Xamarin             : 


