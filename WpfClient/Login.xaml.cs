using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public Login()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Username = txtUser.Text;
            Password = txtPassword.Password;
            HttpClient httpClient = new HttpClient();

            var data = new
            {
                username = Username,
                password = Password
            };
            string json = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync("http://localhost:8080/app/user", content);

                if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(responseBody);
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
    }
}
