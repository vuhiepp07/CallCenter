using CallCenter.Models;
using CallCenter.Pages;
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
using System.Windows.Shapes;

namespace CallCenter.Windows
{
    /// <summary>
    /// Interaction logic for StaffInputWindow.xaml
    /// </summary>
    public partial class StaffInputWindow : Window
    {
        private string accessToken;
        private string signUpStaffUrl = "https://ubercloneserver.herokuapp.com/staff/signup";
        public DataTransferDelegateStaff dataTransferDelegateStaff;
        private readonly List<string> rolesArr = new List<string>() { "ADMIN", "CALLSTAFF", "TRIPSTAFF" };

        public StaffInputWindow()
        {
            InitializeComponent();
            roleCombobox.ItemsSource = rolesArr;
        }

        public StaffInputWindow(DataTransferDelegateStaff del, string accessToken)
        {
            this.accessToken = accessToken;
            InitializeComponent();
            dataTransferDelegateStaff = del;
            roleCombobox.ItemsSource = rolesArr;
        }

        private void addStaffBtn_Click(object sender, RoutedEventArgs e)
        {
            var temp = new {username = staffUserName.Text, password = staffPassword.Password.ToString(), type = roleCombobox.SelectedItem.ToString()};
            string json = JsonConvert.SerializeObject(temp);
            MessageBox.Show(json);
            HttpRequest httpRequest = new HttpRequest();
            string responseContent = httpRequest.PostAsyncJson(signUpStaffUrl, json, accessToken);
            MessageBox.Show(responseContent);
            JObject objTemp = JObject.Parse(responseContent);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            //MessageBox.Show(status);
            if (status.Equals("True") && message.Equals("Signup successfully"))
            {
                dataTransferDelegateStaff?.Invoke(true);
            }
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            dataTransferDelegateStaff?.Invoke(false);
            this.Close();
        }

    }
}
