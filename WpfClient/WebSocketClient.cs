using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Net.WebSockets;

namespace WpfClient
{
    internal class WebSocketClient
    {
        private ClientWebSocket _client;

        public event Action<string> OnMessageReceived;

        public WebSocketClient()
        {
            _client = new ClientWebSocket();
        }

        public async Task ConnectAsync(Uri uri)
        {
            await _client.ConnectAsync(uri, CancellationToken.None);
            await ReceiveMessages();
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024 * 4];
            while (_client.State == WebSocketState.Open)
            {
                var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                OnMessageReceived?.Invoke(message);
            }
        }

        public async Task SendMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await _client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
