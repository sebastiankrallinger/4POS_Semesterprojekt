using Newtonsoft.Json;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User currentUser;
        private Chat activeChat;
        private ClientWebSocket webSocket;
        public MainWindow(object user)
        {
            //Icon setzen
            /*BitmapImage icon = new();
            icon.BeginInit();
            icon.UriSource = new Uri("", UriKind.RelativeOrAbsolute);
            icon.EndInit();
            Icon = BitmapFrame.Create(icon);*/
            InitializeComponent();
            currentUser = (User)user;
            loadChats();
            connectWebSocket();
        }

        private async void connectWebSocket()
        {
            webSocket = new ClientWebSocket();
            try
            {
                await webSocket.ConnectAsync(new Uri("ws://localhost:8080/ws"), CancellationToken.None);
                //MessageBox.Show("Connected to the server.\n");
                await ReceiveMessages();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}\n");
            }
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    //MessageBox.Show("WebSocket closed.\n");
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    //MessageBox.Show($"Received: {message}\n");
                    if(message == "newChat")
                    {
                        loadChats();
                    }else if (message == "newMsg")
                    {
                        HttpClient httpClient = new HttpClient();
                        string url = $"http://localhost:8080/app/users/{currentUser.id}/chat/{activeChat.bezeichnung}";
                        HttpResponseMessage response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        activeChat = JsonConvert.DeserializeObject<Chat>(responseBody);

                        LstBoxMsgs.Items.Clear();
                        List<Message> msgs = activeChat.messages;
                        foreach (Message m in msgs)
                        {
                            LstBoxMsgs.Items.Add(m.message);
                        }
                    }
                }
            }
        }

        public async void loadChats()
        {
            HttpClient httpClient = new HttpClient();
            string url = $"http://localhost:8080/app/users/{currentUser.id}/chats";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();


                List<Chat> chats = JsonConvert.DeserializeObject<List<Chat>>(responseBody);

                LstBoxChats.Items.Clear();
                if (chats != null)
                {
                    foreach (Chat c in chats)
                    {
                        LstBoxChats.Items.Add(c.bezeichnung);
                    }
                }
                
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der Chats: {ex.Message}");
            }

        }

        private async void LstBoxChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtNewMsg.Visibility = Visibility.Visible;
            btnSend.Visibility = Visibility.Visible;
            HttpClient httpClient = new HttpClient();
            string url = $"http://localhost:8080/app/users/{currentUser.id}/chats";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                List<Chat> chats = JsonConvert.DeserializeObject<List<Chat>>(responseBody);
                foreach (Chat c in chats)
                {
                    if (e.AddedItems[0].ToString() == c.bezeichnung)
                    {
                        activeChat = c;
                        LstBoxMsgs.Items.Clear();
                        List<Message> msgs = activeChat.messages;
                        if (msgs != null)
                        {
                            foreach (Message m in msgs)
                            {
                                LstBoxMsgs.Items.Add(m.message);
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der Messages: {ex.Message}");
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewMsg.Text != null)
            {
                HttpClient httpClient = new HttpClient();
                string url = $"http://localhost:8080/app/addMsg?id={currentUser.id}&chatname={activeChat.bezeichnung}&msg={txtNewMsg.Text}&receiver={activeChat.receiver}";

                try
                {
                    HttpResponseMessage response = await httpClient.PutAsync(url, null);
                    if (response.IsSuccessStatusCode) 
                    {
                        var message = txtNewMsg.Text;
                        var messageBuffer = Encoding.UTF8.GetBytes(message);
                        var segment = new ArraySegment<byte>(messageBuffer);

                        try
                        {
                            await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                            //MessageBox.Show($"Sent: {message}\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Send error: {ex.Message}\n");
                        }
                        url = $"http://localhost:8080/app/users/{currentUser.id}/chat/{activeChat.bezeichnung}";
                        response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        activeChat = JsonConvert.DeserializeObject<Chat>(responseBody);

                        LstBoxMsgs.Items.Clear();
                        List<Message> msgs = activeChat.messages;
                        foreach (Message m in msgs)
                        {
                            LstBoxMsgs.Items.Add(m.message);
                        }
                    }

                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Fehler beim Senden der Messages: {ex.Message}");
                }
            }
            txtNewMsg.Clear();
        }

        private async void btnAddChat_Click(object sender, RoutedEventArgs e)
        {
            if (txtReceiver.Text != null && txtChat != null) 
            {
                string receiver = txtReceiver.Text;
                string chatName = txtChat.Text;
                HttpClient httpClient = new HttpClient();
                string url = $"http://localhost:8080/app/addChat?userId={currentUser.id}&chatName={chatName}&receiver={receiver}";

                try
                {
                    HttpResponseMessage response = await httpClient.PutAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var message = chatName;
                        var messageBuffer = Encoding.UTF8.GetBytes(message);
                        var segment = new ArraySegment<byte>(messageBuffer);

                        try
                        {
                            await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                            //MessageBox.Show($"Sent: {message}\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Send error: {ex.Message}\n");
                        }
                        txtChat.Clear();
                        txtReceiver.Clear();
                        url = $"http://localhost:8080/app/users/{currentUser.id}/chat/{chatName}";
                        response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        activeChat = JsonConvert.DeserializeObject<Chat>(responseBody);
                        LstBoxChats.Items.Add(activeChat.bezeichnung);
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Fehler beim erstellen des Chats: {ex.Message}");
                }
            }
        }
    }
}