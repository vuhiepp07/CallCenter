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
    /// Interaction logic for AddDiscount.xaml
    /// </summary>
    public partial class AddDiscount : Window
    {
        private string addDiscountURL = "https://ubercloneserver.herokuapp.com/staff/addDiscount";
        public AddDiscount()
        {
            InitializeComponent();
        }

        private void addDiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            Discount temp = new Discount()
            {
                discountName = discountName.Text,
                discountId = "",
                discountPercent = float.Parse(discountPercent.Text),
                startDate = startDatePicker.Text,
                endDate = endDatePicker.Text,
                quantity = discountQuantity.Text
            };
            string json = JsonConvert.SerializeObject(temp);

            HttpRequest httpRequest = new HttpRequest();
            string responseContent = httpRequest.PostAsyncJson(addDiscountURL, json);
            //MessageBox.Show(responseContent);
            JObject objTemp = JObject.Parse(responseContent);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Add discount successfully"))
            {
                MessageBox.Show("Add discount successfully");
            }
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
