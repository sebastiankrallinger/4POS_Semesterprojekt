using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace WpfClient
{
    public partial class Login : Window
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public Login()
        {
            InitializeComponent();
        }

        //Logik Login Button 
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtUser.Text != "" && txtPassword.Password != "")
            {
                Username = txtUser.Text;
                Password = txtPassword.Password;

                HttpClient httpClient = new HttpClient();

                //Datenpaket fuer Server
                var data = new
                {
                    username = Username,
                    password = Password
                };

                string json = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    //Daten zum Server senden
                    HttpResponseMessage response = await httpClient.PostAsync("http://localhost:8080/app/user", content);

                    if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        User user = JsonConvert.DeserializeObject<User>(responseBody);

                        //Hauptfenster laden
                        MainWindow mainWindow = new MainWindow(user);
                        mainWindow.Show();

                        this.Close();
                    }
                    else
                    {
                        throw new Exception("Falsches Passwort!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ein Fehler ist aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ungültige oder unvollständige Anmeldedaten!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
