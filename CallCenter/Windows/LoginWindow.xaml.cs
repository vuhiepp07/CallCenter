using CallCenter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace CallCenter.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            //string username = userNameTextBox.Text;
            //string password = passwordBox.Password.ToString();
            //Dictionary<string, string> map = new Dictionary<string, string>();
            //map.Add("username", username);
            //map.Add("password", password);
            //string json = JsonConvert.SerializeObject(map);

            //HttpRequest httpRequest = new HttpRequest();
            //string responseContent = httpRequest.PostAsyncJson("https://ubercloneserver.herokuapp.com/staff/login", json);
            ////MessageBox.Show(responseContent);

            ////responseContent.Wait();
            ////string a = responseContent.Result;
            //Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
            //MessageBox.Show(values["status"]);

            //CallCenterWindow callCenterWindow = new CallCenterWindow();
            //callCenterWindow.Show();

            AdministratorWindow administratorWindow = new AdministratorWindow();
            administratorWindow.Show();
            this.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
