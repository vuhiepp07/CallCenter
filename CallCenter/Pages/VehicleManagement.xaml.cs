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
    /// Interaction logic for VehicleManagement.xaml
    /// </summary>
    public partial class VehicleManagement : Page
    {
        private const string GetAllVehicleUrl = "https://ubercloneserver.herokuapp.com/staff/getAllVehicle";

        IList<Vehicle> vehicles = new List<Vehicle>();
        List<Vehicle> VehiclesListToView = new List<Vehicle> { };
        List<Vehicle> SelectedVehicles = new List<Vehicle> { };
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        private CollectionViewSource VehicleViewSource;

        private void refreshViewSource(IList<Vehicle> vehicles)
        {
            VehiclesListToView = (List<Vehicle>)vehicles;
            SelectedVehicles = VehiclesListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            VehicleViewSource.Source = SelectedVehicles;
            _currentPage = 1;
            _totalItems = vehicles.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }
        public void getAndBindingVehicleData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsync(GetAllVehicleUrl);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            vehicles = arr.ToObject<List<Vehicle>>();
            refreshViewSource(vehicles);
        }

        public VehicleManagement()
        {
            InitializeComponent();
            VehicleViewSource = (CollectionViewSource)FindResource(nameof(VehicleViewSource));
            getAndBindingVehicleData();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //var ownerName = SearchField.Text.Trim().ToLower();
            //SearchField.Text = "";
            //VehicleViewSource.Source = from vehicle in vehicles
            //                           where vehicle.ownerName.ToLower() == ownerName.ToLower()
            //                           select vehicle;
            refreshViewSource((IList<Vehicle>)VehicleViewSource.Source);
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingVehicleData();
        }

        private void PrevVehiclePageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedVehicles = VehiclesListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                VehicleViewSource.Source = SelectedVehicles;
            }
        }

        private void NextVehiclePageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedVehicles = VehiclesListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                VehicleViewSource.Source = SelectedVehicles;
            }
        }

        private void ViewAddVehicleRequestListbtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/AddVehicleManagement.xaml", UriKind.Relative));
        }
    }
}
