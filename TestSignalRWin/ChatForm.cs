using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;

namespace TestSignalRWin
{
    public partial class ChatForm : Form
    {
        private HubConnection _connection;

        //-----------------------------------------------------
        // init
        //-----------------------------------------------------
        public ChatForm()
        {
            InitializeComponent();
        }
        private void ChatForm_Load(object sender, EventArgs e)
        {
            tbAddr.Focus();
        }

        //-----------------------------------------------------
        // connect  
        //-----------------------------------------------------
        private void tbAddr_Enter(object sender, EventArgs e)
        {
            AcceptButton = btnConnect;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            UpdateState(connected: false);
            _connection = new HubConnectionBuilder().WithUrl(tbAddr.Text).Build();
            _connection.On<string, string>("broadcastMessage", (s1, s2) => OnSend(s1, s2));
            Log(Color.Gray, "Starting connection...");
            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.ToString());
                return;
            }

            Log(Color.Gray, "Connection established.");
            UpdateState(connected: true);
            tbMessage.Focus();
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            Log(Color.Gray, "Stopping connection...");
            try
            {
                await _connection.StopAsync();
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.ToString());
            }
            Log(Color.Gray, "Connection terminated.");
            UpdateState(connected: false);
        }

        private void UpdateState(bool connected)
        {
            btnDisconnect.Enabled = connected;
            btnConnect.Enabled = !connected;
            tbAddr.Enabled = !connected;
            tbMessage.Enabled = connected;
            btnSend.Enabled = connected;
        }

        //-----------------------------------------------------
        // send
        //-----------------------------------------------------
        private void tbMessage_Enter(object sender, EventArgs e)
        {
            AcceptButton = btnSend;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                // 发送消息给服务器端（消息名称、发送者、消息内容）
                await _connection.InvokeAsync("Broadcast", tbUser.Text, tbMessage.Text);
            }
            catch (Exception ex)
            {
                Log(Color.Red, ex.ToString());
            }
        }

        private void OnSend(string name, string message)
        {
            Log(Color.Black, name + ": " + message);
        }


        //-----------------------------------------------------
        // log
        //-----------------------------------------------------
        private void Log(Color color, string message)
        {
            Action callback = () =>
            {
                lbMessage.Items.Add(new LogMessage(color, message));
            };
            Invoke(callback);
        }

        private class LogMessage
        {
            public Color MessageColor { get; }
            public string Content { get; }
            public LogMessage(Color messageColor, string content)
            {
                MessageColor = messageColor;
                Content = content;
            }
        }

        private void messagesList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            var message = (LogMessage)lbMessage.Items[e.Index];
            e.Graphics.DrawString(
                message.Content,
                lbMessage.Font,
                new SolidBrush(message.MessageColor),
                e.Bounds
                );
        }
    }
}
