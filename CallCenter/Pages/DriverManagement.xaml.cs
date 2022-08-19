using CallCenter.Models;
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
        List<Vehicle> vehiclesOfSpecificDrivers = new List<Vehicle>();
        private void refreshViewSource(IList<Driver> drivers)
        {
            DriversListToView = (List<Driver>)drivers;
            SelectedDrivers = DriversListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            DriverViewSource.Source = SelectedDrivers;
            _currentPage = 1;
            _totalItems = drivers.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }
        public void getAndBindingDriverData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(GetAllDriverUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            drivers = arr.ToObject<List<Driver>>();
            refreshViewSource(drivers);
        }

        public DriverManagement()
        {
            InitializeComponent();
            DriverViewSource = (CollectionViewSource)FindResource(nameof(DriverViewSource));
            getAndBindingDriverData();
        }



        IList<Driver> drivers = new List<Driver>();
        List<Driver> DriversListToView = new List<Driver> { };
        List<Driver> SelectedDrivers = new List<Driver> { };
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        private CollectionViewSource DriverViewSource;

        private void btnLatestTrips_Click(object sender, RoutedEventArgs e)
        {
            User temp = (User)driverListView.SelectedItem;
            string tempUrl = getAllTripOfDriverUrl + temp.id;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Delete staff successfully"))
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
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedDrivers = DriversListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                DriverViewSource.Source = SelectedDrivers;
            }
        }


        private void PrevDriverPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedDrivers = DriversListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                DriverViewSource.Source = SelectedDrivers;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var fullname = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            DriverViewSource.Source = from driver in drivers
                                      where driver.fullname.ToLower() == fullname.ToLower()
                                      select driver;
            refreshViewSource((IList<Driver>)DriverViewSource.Source);
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
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            //MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            vehiclesOfSpecificDrivers = arr.ToObject<List<Vehicle>>();

            this.NavigationService.Navigate(new VehicleManagement(vehiclesOfSpecificDrivers));
        }
    }
}
