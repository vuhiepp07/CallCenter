using CallCenter.Pages;
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

namespace CallCenter
{
    /// <summary>
    /// Interaction logic for TripTrackingWindow.xaml
    /// </summary>
    public partial class TripTrackingWindow : Window
    {
        private string accessToken;
        private string username;
        public TripTrackingWindow()
        { 
            InitializeComponent();
        }

        public TripTrackingWindow(string token, string username)
        {
            InitializeComponent();
            accessToken = token;
            this.username = username;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NavigateToTripManagement(object sender, RoutedEventArgs e)
        {
            Main.Content = new TripManagement();
            Main.NavigationService.RemoveBackEntry();
        }

        private void NavigateToVehicleManagement(object sender, RoutedEventArgs e)
        {
            Main.Content = new VehicleManagement();
            Main.NavigationService.RemoveBackEntry();
        }

        private void NavigateToConfigurationPage(object sender, RoutedEventArgs e)
        {
            Main.Content = new ConfigurationPage(accessToken, username);
            Main.NavigationService.RemoveBackEntry();
        }
    }
}
