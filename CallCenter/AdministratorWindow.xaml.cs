﻿using CallCenter.Pages;
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
    /// Interaction logic for AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        private string username;
        private string accessToken;
        public AdministratorWindow()
        {
            InitializeComponent();
        }

        public AdministratorWindow(string token, string usname)
        {
            InitializeComponent();
            accessToken = token;
            username = usname;
        }

        private void NavigateToRequestStatistic(object sender, RoutedEventArgs e)
        {
            Main.Content = new RequestStatistic();
            Main.NavigationService.RemoveBackEntry();
        }

        private void NavigateToDriverStatistic(object sender, RoutedEventArgs e)
        {
            Main.Content = new DriverStatistic();
            Main.NavigationService.RemoveBackEntry();
        }

        private void NavigateToInComeAndTripReport(object sender, RoutedEventArgs e)
        {
            Main.Content = new IncomeAndNumOfTripReport();
            Main.NavigationService.RemoveBackEntry();
        }

        private void NavigateToStaffManagement(object sender, RoutedEventArgs e)
        {
            Main.Content = new StaffManagement(accessToken);
            Main.NavigationService.RemoveBackEntry();
        }

        private void NavigateToConfigurationPage(object sender, RoutedEventArgs e)
        {
            Main.Content = new ConfigurationPage(accessToken, username);
            Main.NavigationService.RemoveBackEntry();
        }
    }
}
