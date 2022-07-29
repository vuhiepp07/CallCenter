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
        string role, accessToken;
        private string loginURL = "https://ubercloneserver.herokuapp.com/staff/login";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            //CallCenterWindow callCenterWindow = new CallCenterWindow(accessToken);
            //callCenterWindow.Show();

            string username = userNameTextBox.Text;
            string password = passwordBox.Password.ToString();
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("username", username);
            map.Add("password", password);
            string json = JsonConvert.SerializeObject(map);

            HttpRequest httpRequest = new HttpRequest();
            string responseContent = httpRequest.PostLoginJson(loginURL, json);
            MessageBox.Show(responseContent);
            JObject objTemp = JObject.Parse(responseContent);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            //MessageBox.Show(status);
            if (status.Equals("True") && message.Equals("Login successfully"))
            {
                role = (string)objTemp["data"]["role"];
                accessToken = (string)objTemp["data"]["token"];

                AccountnTokenHelper.userName = username;
                AccountnTokenHelper.accessToken = accessToken;

                if (role == "ADMIN")
                {
                    AdministratorWindow administratorWindow = new AdministratorWindow();
                    administratorWindow.Show(); 
                }
                else if (role == "CALLSTAFF")
                {
                    CallCenterWindow callCenterWindow = new CallCenterWindow();
                    callCenterWindow.Show();
                }
                else if (role == "TRIPSTAFF")
                {
                    TripTrackingWindow tripTrackingWindow = new TripTrackingWindow();
                    tripTrackingWindow.Show();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid account, please login again");
                userNameTextBox.Clear();
                passwordBox.Clear();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
