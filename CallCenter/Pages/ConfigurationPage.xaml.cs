using CallCenter.Models;
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
        private string changePasswordUrl = "https://ubercloneserver.herokuapp.com/staff/changePassword";
        public ConfigurationPage()
        {
            InitializeComponent();
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
                type = StaffTypeCBox.Text
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
            }
            else
            {
                MessageBox.Show("Update staff information failed, please try again");
            }
        }
    }
}
