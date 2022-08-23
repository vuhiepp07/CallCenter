using CallCenter.Models;
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
    /// Interaction logic for TripTracking.xaml
    /// </summary>
    public partial class TripTracking : Page
    {
        private string mapViewPlaceUrl = "https://www.google.com/maps/place/";
    
    public TripTracking()
        {
            InitializeComponent();
        }

        public TripTracking(string start, string end, string vehicleType, string paymentType, string userid, string driverid, point currentLocation)
        {
            InitializeComponent();
            StartTxtBox.Text = start;
            EndTxtBox.Text = end;
            paymentTypeTextBox.Text = paymentType;
            VehileTypeTxtBox.Text = vehicleType;
            Ggmap.Source = new Uri(mapViewPlaceUrl + currentLocation.latitude +"," + currentLocation.longitude);
            userIdTextBox.Text = userid;
            driverIdTextBox.Text = driverid;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TripManagement());
        }
    }
}
