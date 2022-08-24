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
using System.Windows.Shapes;

namespace CallCenter.Windows
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private string changePasswordUrl = "https://ubercloneserver.herokuapp.com/staff/changePassword";
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxNewPassword.Password.ToString().Equals(txtBoxConfirmNewPassword.Password.ToString())){
                HttpRequest httpRequest = new HttpRequest();
                var temp = new {username = AccountnTokenHelper.userName, password = txtBoxOldPassword.Password.ToString(), newPassword= txtBoxNewPassword.Password.ToString()};
                string json = JsonConvert.SerializeObject(temp);
                string responseContent = httpRequest.PostJsonWithAccessToken(changePasswordUrl, json, AccountnTokenHelper.accessToken);
                //MessageBox.Show(responseContent);
                JObject objTemp = JObject.Parse(responseContent);
                string status = (string)objTemp["status"];
                string message = (string)objTemp["message"];
                if(status.Equals("True") && message.Equals("Change password successfully"))
                {
                    MessageBox.Show("Change password successfully");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("New password and Confirm new password fields have to be the same, please try again");
                txtBoxNewPassword.Clear();
                txtBoxConfirmNewPassword.Clear();
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtBoxUserName.IsReadOnly = true;
            txtBoxUserName.Text = AccountnTokenHelper.userName;
        }
    }
}
