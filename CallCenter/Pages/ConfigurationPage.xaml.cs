using CallCenter.Models;
using CallCenter.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CallCenter.Pages
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        private string getStaffInfoUrl = "https://ubercloneserver.herokuapp.com/staff/infor";
        private string changePasswordUrl = "https://ubercloneserver.herokuapp.com/staff/changePassword";
        List<string> genders = new List<string> { "male", "female" };
        public ConfigurationPage()
        {
            InitializeComponent();
        }

        private void OnOff(Boolean status)
        {
            FullNameTextBox.IsReadOnly = status;
            PhoneTextBox.IsReadOnly = status;
            AddressTextBox.IsReadOnly= status;
            EmailTextBox.IsReadOnly = status;
            GenderCBox.IsReadOnly = status;
            btnUpdate.IsEnabled = !status;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Staff temp = new Staff
            {
                username = UsernameTextBox.Text,
                id = StaffIDTextBox.Text,
                fullname = FullNameTextBox.Text,
                phone = PhoneTextBox.Text,
                address = AddressTextBox.Text,
                email = EmailTextBox.Text,
                gender = GenderCBox.Text,
                type = StaffTypeTextBox.Text
            };

            string json = JsonConvert.SerializeObject(temp);

            HttpRequest httpRequest = new HttpRequest();
            string responseContent = httpRequest.PostAsyncJsonWithAccessToken( changePasswordUrl, json, AccountnTokenHelper.accessToken);
            MessageBox.Show(responseContent);
            JObject objTemp = JObject.Parse(responseContent);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];

            if (message.Equals("") && status.Equals("True"))
            {
                MessageBox.Show("Update staff information successfully");
                Canvas_Loaded(sender, e);
            }
            else
            {
                MessageBox.Show("Update staff information failed, please try again");
            }
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameTextBox.IsReadOnly = true;
            StaffIDTextBox.IsReadOnly = true;
            StaffTypeTextBox.IsReadOnly = true;
            GenderCBox.ItemsSource = genders;
            OnOff(true);


            var temp = new { username = AccountnTokenHelper.userName };

            string json = JsonConvert.SerializeObject(temp);
            HttpRequest httpRequest = new HttpRequest();
            string responecontent = httpRequest.GetDataFromUrlAsyncWithAccessTokenAndJson(getStaffInfoUrl, json , AccountnTokenHelper.accessToken);
            MessageBox.Show(responecontent);
            JObject objTemp = JObject.Parse(responecontent);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if(status.Equals("True")&& message.Equals("Get staff successfully"))
            {
                UsernameTextBox.Text = (string)objTemp["data"]["username"];
                StaffIDTextBox.Text = (string)objTemp["data"]["id"];
                FullNameTextBox.Text = (string)objTemp["data"]["fullname"];
                PhoneTextBox.Text = (string)objTemp["data"]["phone"];
                AddressTextBox.Text = (string)objTemp["data"]["address"];
                EmailTextBox.Text = (string)objTemp["data"]["email"];
                GenderCBox.Text = (string)objTemp["data"]["gender"];
                StaffTypeTextBox.Text = (string)objTemp["data"]["type"];
            }
            else
            {

            }

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            OnOff(false);
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow();
            changePasswordWindow.ShowDialog();
        }
    }
}
