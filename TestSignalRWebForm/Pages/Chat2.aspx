<%@ Page MasterPageFile="~/Site.Master"  %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<script src="/Scripts/jquery.signalR-2.4.1.min.js" ></script>
<link href="https://cdn.bootcdn.net/ajax/libs/twitter-bootstrap/4.5.3/css/bootstrap-grid.css" rel="stylesheet">

<div class="row">
    <div class="col-6">
        <label>用户</label>
        <input type="text" id="userName" list="users" class="form-control" />
        <datalist id="users">
            <option value="Kevin">
            <option value="Cherry">
            <option value="Melissa">
        </datalist>
        <input type="button" id="btnConnect" value="登录" class="btn" />
        <input type="button" id="btnDisconnect" value="注销" class="btn" />
        <input type="button" id="btnExit" value="断开" class="btn" hidden="hidden" />
        <br />

        <label>组名</label>
        <input type="text" id="group" list="groups" class="form-control" />
        <datalist id="groups">
            <option value="Group1">
            <option value="Group2">
            <option value="Group3">
        </datalist>
        <input type="button" id="btnJoinGroup" value="加入组" class="btn" />
        <input type="button" id="btnQuitGroup" value="退出组" class="btn" />
        <br />

        <label>用户ID</label>
        <input type="text" id="target" class="form-control" />
        <label for="msg">消息</label>
        <input type="text" id="msg" class="form-control" />
        <input type="button" id="btnTalkTo" value="私信" class="btn" />
        <input type="button" id="btnTalkToGroup" value="组播" class="btn" />
        <input type="button" id="btnTalkToAll" value="广播" class="btn" />
    </div>

    <div  class="col-6">
        <label>消息</label>
        <ul id="messages"></ul>
    </div>
</div>
    <script type="text/javascript">
        $(function () {
            // SignalR connection
            var connection = $.connection('/Chat');
            connection.received(function (data) {
                // 如果服务器端发送的是对象，客户端会自动解析为对象。否则要自己处理一下。
                var o = JSON.parse(data); 
                var item = "";
                if (o.FromID != null && o.ToID != null)
                    item = stringFormat("<li>[{0}] {1} say to {2} : {3}</li>", o.Date, o.FromName, o.ToName, o.Data);
                if (o.FromID != null && o.ToID == null)
                    item = stringFormat("<li>[{0}] {1}: {2} ({3})</li>", o.Date, o.FromName, o.Data, o.Type);
                if (o.FromID == null && o.ToID == null && o.Data != null)
                    item = stringFormat("<li>[{0}] {1} ({2})</li>", o.Date, o.Data, o.Type);
                if (o.FromID == null && o.ToID == null && o.Data == null)
                    item = stringFormat("<li>[{0}] ({1})</li>", o.Date, o.Type);
                $('#messages').append(item);
            });
            connection.start();

            // 发送信息到服务器端
            function send(o) { connection.send(JSON.stringify(o)); }
            function connect(name) { send({ Type: 'Connect', Name: name }); }
            function disconnect(name) { send({ Type: 'Disconnect', Name: name }); }
            function joinGroup(group) { send({ Type: 'JoinGroup', To: group }); }
            function quitGroup(group) { send({ Type: 'QuitGroup', To: group }); }
            function talkTo(to, msg) { send({ Type: 'TalkToOne', To: to, Data: msg }); }
            function talkToAll(msg) { send({ Type: 'TalkToAll', Data: msg }); }
            function talkToGroup(group, msg) { send({ Type: 'TalkToGroup', To: group, Data: msg }); }

            // 按钮事件
            $("#btnExit").click(function () { connection.stop(); });
            $("#btnConnect").click(function () { connect($("#userName").val()); });
            $("#btnDisconnect").click(function () { disconnect($("#userName").val()); });
            $("#btnJoinGroup").click(function () { joinGroup($("#group").val()); });
            $("#btnQuitGroup").click(function () { quitGroup($("#group").val()); });
            $("#btnTalkTo").click(function () { talkTo($("#target").val(), $("#msg").val()); });
            $("#btnTalkToGroup").click(function () { talkToGroup($("#group").val(), $("#msg").val()); });
            $("#btnTalkToAll").click(function () { talkToAll($("#msg").val()); });
        });

        // 字符串格式化
        function stringFormat() {
            if (arguments.length == 0)
                return null;
            var str = arguments[0];
            for (var i = 1; i < arguments.length; i++) {
                var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
                str = str.replace(re, arguments[i]);
            }
            return str;
        }
    </script>

</asp:Content>