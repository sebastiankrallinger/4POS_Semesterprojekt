using Newtonsoft.Json;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfClient
{
    public partial class MainWindow : Window
    {
        private User currentUser;
        private Chat activeChat;
        private ClientWebSocket webSocket;
        private Style receivedStyle;
        private Style sentStyle;

        public MainWindow(object user)
        {
            InitializeComponent();
            currentUser = (User)user;
            loadChats();
            connectWebSocket();
        }

        //Verbindung zum Websocket herstellen
        private async void connectWebSocket()
        {
            webSocket = new ClientWebSocket();

            try
            {
                //Websocket verbinden
                await webSocket.ConnectAsync(new Uri("ws://localhost:8080/ws"), CancellationToken.None);
                //MessageBox.Show("Connected to the server.\n");
                await ReceiveMessages();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Verbindung zum Websocket fehlgeschlagen: {ex.Message}\n");
            }
        }

        //Nachrichten vom Websocket empfangen
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

                    //WS-Nachricht verarbeiten
                    if(message == "newChat")
                    {
                        loadChats();
                    }else if (message == "newMsg")
                    {
                        //Messages neu laden
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
                            MessageItem messageItem = new MessageItem(m.message, m.receiver, m.date);
                            LstBoxMsgs.Items.Add(messageItem);
                        }
                    }
                }
            }
        }

        //Chats aus der DB laden und anzeigen
        public async void loadChats()
        {
            HttpClient httpClient = new HttpClient();
            string url = $"http://localhost:8080/app/users/{currentUser.id}/chats";

            try
            {
                //Daten aus DB laden
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();


                List<Chat> chats = JsonConvert.DeserializeObject<List<Chat>>(responseBody);

                //GUI aktualisieren
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

        //Nachrichten des ausgewählten Chats anzeigen
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

                //GUI aktualisieren
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
                                MessageItem messageItem = new MessageItem(m.message, m.receiver, m.date);
                                LstBoxMsgs.Items.Add(messageItem);
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


        //Nachricht senden
        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewMsg.Text != null)
            {
                HttpClient httpClient = new HttpClient();
                string url = $"http://localhost:8080/app/addMsg?id={currentUser.id}&chatname={activeChat.bezeichnung}&msg={txtNewMsg.Text}&receiver={activeChat.receiver}";

                try
                {                
                    //Message an Server senden
                    HttpResponseMessage response = await httpClient.PostAsync(url, null);
                    if (response.IsSuccessStatusCode) 
                    {
                        var message = "newMsg";
                        var messageBuffer = Encoding.UTF8.GetBytes(message);
                        var segment = new ArraySegment<byte>(messageBuffer);

                        try
                        {
                            //Nachricht an Websocket senden
                            await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                            //MessageBox.Show($"Sent: {message}\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Send error: {ex.Message}\n");
                        }

                        url = $"http://localhost:8080/app/users/{currentUser.id}/chat/{activeChat.bezeichnung}";

                        //aktualisierte Messages laden
                        response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        activeChat = JsonConvert.DeserializeObject<Chat>(responseBody);

                        //GUI aktualisieren
                        LstBoxMsgs.Items.Clear();
                        List<Message> msgs = activeChat.messages;
                        foreach (Message m in msgs)
                        {
                            MessageItem messageItem = new MessageItem(m.message, m.receiver, m.date);
                            LstBoxMsgs.Items.Add(messageItem);
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

        //neuen Chat hinzufuegen
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
                    //Daten an Server senden
                    HttpResponseMessage response = await httpClient.PostAsync(url, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var message = "newChat";
                        var messageBuffer = Encoding.UTF8.GetBytes(message);
                        var segment = new ArraySegment<byte>(messageBuffer);

                        try
                        {
                            //Nachricht an Websocket senden
                            await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                            //MessageBox.Show($"Sent: {message}\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Send error: {ex.Message}\n");
                        }

                        txtChat.Clear();
                        txtReceiver.Clear();

                        //aktualisierte Chats aus DB laden
                        url = $"http://localhost:8080/app/users/{currentUser.id}/chat/{chatName}";
                        response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        activeChat = JsonConvert.DeserializeObject<Chat>(responseBody);

                        //GUI aktualisieren
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