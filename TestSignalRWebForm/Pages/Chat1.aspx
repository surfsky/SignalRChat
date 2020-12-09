<%@ Page MasterPageFile="~/Site.Master"  %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<script src="/Scripts/jquery.signalR-2.4.1.min.js" ></script>
<input type="text" id="msg" />
<input type="button" id="broadcast" value="发送" />
<input type="button" id="btnStop" value="停止" />
<ul id="messages"></ul>


<script type="text/javascript">
    $(function () {
        var connection = $.connection('/Chats');
        connection.received(function (data) {
            $('#messages').append('<li>' + data + '</li>');
        });
        connection.start();

        $("#broadcast").click(function () {connection.send($('#msg').val());});
        $("#btnStop").click(function ()   {connection.stop();});
    });
</script>

</asp:Content>