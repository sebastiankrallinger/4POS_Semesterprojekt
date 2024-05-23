using Apache.NMS;
using Newtonsoft.Json;
using System.Net.Http;
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
                        foreach (Message m in msgs)
                        {
                            LstBoxMsgs.Items.Add(m.message);
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
                        url = $"http://localhost:8080/app/users/{currentUser.id}/chat/{activeChat.bezeichnung}";
                        response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        activeChat = JsonConvert.DeserializeObject<Chat>(responseBody);

                        LstBoxMsgs.Items.Clear();
                        List<Message> msgs = activeChat.messages;
                        foreach (Message m in msgs)
                        {
                            if (m.receiver == false)
                            {
                                LstBoxMsgs.Items.Add(m.message);
                            }
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