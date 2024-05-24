using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using WebSocket4Net;

namespace WpfClient
{
    internal class WebSocketClient
    {
        private WebSocket ws;

        public event Action<string> OnMessageReceived;

        public void Connect(string uri)
        {
            ws = new WebSocket(uri);
            ws.Opened += OnOpened;
            ws.Closed += OnClosed;
            ws.MessageReceived += OnMessageReceivedHandler;
            ws.Open();
        }

        private void OnOpened(object sender, EventArgs e)
        {
            MessageBox.Show("Connected to WebSocket server");
            Send("CONNECT\naccept-version:1.1,1.2\n\n\0");
        }

        private void OnClosed(object sender, EventArgs e)
        {
            MessageBox.Show("Disconnected from WebSocket server");
        }

        private void OnMessageReceivedHandler(object sender, MessageReceivedEventArgs e)
        {
            OnMessageReceived?.Invoke(e.Message);
        }

        public void Subscribe(string destination)
        {
            Send($"SUBSCRIBE\ndestination:{destination}\nid:sub-0\n\n\0");
        }

        public void SendMessage(string destination, string message)
        {
            var msg = $"SEND\ndestination:{destination}\n\n{message}\0";
            Send(msg);
        }

        private void Send(string message)
        {
            ws.Send(message);
        }

        public void Disconnect()
        {
            Send("DISCONNECT\n\n\0");
            ws.Close();
        }
    }
}
