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
    /// Interaction logic for CallCenterWindow.xaml
    /// </summary>
    public partial class CallCenterWindow : Window
    {
        private string accessToken;
        public CallCenterWindow()
        {
            InitializeComponent();
        }

        public CallCenterWindow(string token)
        {
            accessToken = token;
            InitializeComponent();
        }

        private void NavigateToUser(object sender, RoutedEventArgs e)
        {
            Main.Content = new UserManagement();
            Main.NavigationService.RemoveBackEntry();
        }

        private void NavigateToDriver(object sender, RoutedEventArgs e)
        {
            Main.Content = new DriverManagement();
            Main.NavigationService.RemoveBackEntry();
        }

        private void NavigateToRequest(object sender, RoutedEventArgs e)
        {
            //Main.Content = new CreateRequest();
            Main.Content = new RequestManagement();
            Main.NavigationService.RemoveBackEntry();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NavigateToDiscount(object sender, RoutedEventArgs e)
        {
            Main.Content = new DiscountManagement();
            Main.NavigationService.RemoveBackEntry();
        }
    }
}
