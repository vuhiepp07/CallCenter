﻿using CallCenter.Models;
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
    /// Interaction logic for DriverManagement.xaml
    /// </summary>
    public partial class DriverManagement : Page
    {
        private const string GetAllDriverUrl = "https://ubercloneserver.herokuapp.com/staff/getAllDriver";
        private string getAllTripOfDriverUrl = "https://ubercloneserver.herokuapp.com/staff/getAllTripByDriver/";
        private string getVehiclesOfSpecificDriverUrl = "https://ubercloneserver.herokuapp.com/staff/getVehicleOfDriver/";
        private PagingHelper<Driver> pagingHelper;
        List<Vehicle> vehiclesOfSpecificDrivers = new List<Vehicle>();
        IList<Driver> drivers = new List<Driver>();

        private CollectionViewSource DriverViewSource;

        private void refreshViewSource()
        {
            pagingHelper = new PagingHelper<Driver>(drivers);
            DriverViewSource.Source = pagingHelper.refreshView();
            PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
        }
        public void getAndBindingDriverData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlWithAccessToken(GetAllDriverUrl, AccountnTokenHelper.accessToken);
            //MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            drivers = arr.ToObject<List<Driver>>();
            refreshViewSource();
        }

        public DriverManagement()
        {
            InitializeComponent();
            DriverViewSource = (CollectionViewSource)FindResource(nameof(DriverViewSource));
            getAndBindingDriverData();
        }

        private void btnLatestTrips_Click(object sender, RoutedEventArgs e)
        {
            Driver temp = (Driver)driverListView.SelectedItem;
            string tempUrl = getAllTripOfDriverUrl + temp.driverId;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            //MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Get all trip successfully"))
            {
                JArray arr = (JArray)objTemp["data"];
                List<Trip> trips = arr.ToObject<List<Trip>>();
                if (trips.Count == 0)
                {
                    MessageBox.Show("This driver haven't drive any trip yet");
                }
                else
                {
                    this.NavigationService.Navigate(new TripManagement(trips));
                }
            }
            else
            {
                MessageBox.Show("Some error occured");
            }
        }


        private void NextDriverPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage < pagingHelper._totalPages)
            {
                DriverViewSource.Source = pagingHelper.nextPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }


        private void PrevDriverPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage > 1)
            {
                DriverViewSource.Source = pagingHelper.prevPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var fullname = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            DriverViewSource.Source = from driver in drivers
                                      where driver.fullname.ToLower() == fullname.ToLower()
                                      select driver;
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingDriverData();
        }

        private void btnViewVehicleOfDriver_Click(object sender, RoutedEventArgs e)
        {
            string driverId = ((Driver)driverListView.SelectedItem).driverId;
            string tempUrl = getVehiclesOfSpecificDriverUrl + driverId;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            //MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            vehiclesOfSpecificDrivers = arr.ToObject<List<Vehicle>>();

            this.NavigationService.Navigate(new VehicleManagement(vehiclesOfSpecificDrivers));
        }
    }
}
