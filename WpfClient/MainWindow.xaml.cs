﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Icon setzen
            /*BitmapImage icon = new();
            icon.BeginInit();
            icon.UriSource = new Uri("", UriKind.RelativeOrAbsolute);
            icon.EndInit();
            Icon = BitmapFrame.Create(icon);*/
            InitializeComponent();
            loadChats();
        }

        public async void loadChats()
        {
            string userId = "6644bb25694ff165bb4e5b03";
            HttpClient httpClient = new HttpClient();
            string url = $"http://localhost:8080/app/users/{userId}/chats";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                
                List<Chat> chats = JsonConvert.DeserializeObject<List<Chat>>(responseBody);

                LstBoxChats.Items.Clear();

                foreach (Chat c in chats)
                {
                    LstBoxChats.Items.Add(c.bezeichnung);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der Chats: {ex.Message}");
            }
            
        }

        private async void LstBoxChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string userId = "6644bb25694ff165bb4e5b03";
            HttpClient httpClient = new HttpClient();
            string url = $"http://localhost:8080/app/users/{userId}/chats";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                List<Chat> chats = JsonConvert.DeserializeObject<List<Chat>>(responseBody);

                foreach (Chat c in chats)
                {
                    if(c.bezeichnung == e.AddedItems[0].ToString())
                    {
                        LstBoxMsgs.Items.Clear();
                        List<Message> msgs = c.messages;
                        foreach(Message m in msgs)
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
    }
}