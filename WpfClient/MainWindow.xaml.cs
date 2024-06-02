using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Data;
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
        private string previousDate;
        private ClientWebSocket webSocket;
        private Style receivedStyle;
        private Style sentStyle;

        public MainWindow(object user)
        {
            InitializeComponent();
            currentUser = (User)user;
            lblHeadingChats.Content = "Deine Chats - " + currentUser.username;
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
                    loadChats();
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
                        if (c.newMsg == true)
                        {
                            c.BackgroundColor = Brushes.LightGreen;
                        }
                        else if (c.newMsg == false)
                        {
                            c.BackgroundColor = Brushes.Transparent;
                        }
                        LstBoxChats.Items.Add(c);

                        if (activeChat != null && activeChat.bezeichnung == c.bezeichnung)
                        {
                            activeChat = c;
                            showMessges(activeChat);
                        }
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der Chats: {ex.Message}");
            }

        }

        //Nachrichten des ausgewählten Chats anzeigen
        private void showMessges(Chat c)
        {
            LstBoxMsgs.Items.Clear();
            List<Message> msgs = activeChat.messages;
            previousDate = "";

            if (msgs != null)
            {
                foreach (Message m in msgs)
                {
                    string[] dateTime = m.date.Split(" ");
                    string[] dayMonthYear = dateTime[0].Split("-");
                    string formattedDate = dayMonthYear[0] + "." + dayMonthYear[1] + "." + dayMonthYear[2];

                    if (formattedDate != previousDate)
                    {
                        previousDate = formattedDate;
                        MessageItem dateItem = new MessageItem(formattedDate);
                        LstBoxMsgs.Items.Add(dateItem);
                    }

                    MessageItem messageItem = new MessageItem(m.message, m.receiver, m.date);
                    LstBoxMsgs.Items.Add(messageItem);
                }
            }
        }

        //Chatauswahl des Benutzers verwalten
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

                if (e.AddedItems.Count == 0)
                {
                    return;
                }

                Chat item = (Chat)e.AddedItems[0];
                lblHeadMsgs.Content = item.bezeichnung;

                //GUI aktualisieren
                foreach (Chat c in chats)
                {
                    if (item.bezeichnung == c.bezeichnung)
                    {
                        activeChat = c;
                        await updateStatus(c);
                        loadChats();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Fehler beim ausgewählten Chat: {ex.Message}");
            }
        }


        //Nachricht senden
        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewMsg.Text != "")
            {
                HttpClient httpClient = new HttpClient();
                string url = $"http://localhost:8080/app/addMsg?id={currentUser.id}&chatname={activeChat.bezeichnung}&msg={txtNewMsg.Text}&receiver={activeChat.receiver}";

                try
                {
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
                        showMessges(activeChat);
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
                            MessageBox.Show($"Fehler beim Hinzufügen des Chats: {ex.Message}\n");
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
                        LstBoxChats.Items.Add(activeChat);
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Fehler beim erstellen des Chats: {ex.Message}");
                }
            }
        }

        //Chat als gelesen makieren
        private async Task updateStatus(Chat c)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"http://localhost:8080/app/updateStaus?id={currentUser.id}&chatname={c.bezeichnung}";

            try
            {
                await httpClient.PostAsync(url, null);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Fehler beim updaten des Status: {ex.Message}");
            }
        }
    }
}