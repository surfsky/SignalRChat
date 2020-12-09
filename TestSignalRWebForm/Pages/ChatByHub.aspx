<%@ Page MasterPageFile="~/Site.Master"  %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div>
    <label for="tbName">Name</label>
    <input type="text" id="tbName" value="kevin" />
    <label for="tbMsg">Message</label>
    <input type="text" id="tbMsg" value="hi" />
    <input type="button" id="btnSend" value="发送" />
    <input type="button" id="btnStop" value="停止" />
    <ul id="messages"></ul>
</div>


<script src="/Scripts/jquery-3.4.1.min.js"></script>
<script src="/Scripts/jquery.signalR-2.4.1.min.js" ></script>
<script src="/signalr/hubs"></script>
<script type="text/javascript">
    // 为显示的消息进行Html编码
    function htmlEncode(value) {
        return $('<div />').text(value).html();
    }

    $(function () {
        // 客户端方法（供服务器端调用）
        var proxy = $.connection.chatHub;
        proxy.client.showMessage = function (message) {
            $('#messages').append('<li>' + htmlEncode(message) + '</li>');
        };
        $.connection.hub.start();


        // buttons
        $("#btnSend").click(function () {
            var name = $('#tbName').val();
            var msg = $('#tbMsg').val();
            var info = name + " say " + msg;
            proxy.server.broadcast(info);
        });
        $("#btnStop").click(function () {
            $.connection.stop();
        });
    });
</script>

</asp:Content>