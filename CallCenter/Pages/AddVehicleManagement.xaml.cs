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
    /// Interaction logic for AddVehicleManagement.xaml
    /// </summary>
    public partial class AddVehicleManagement : Page
    {
        //private string acceptVehicleUrl = "https://ubercloneserver.herokuapp.com/staff/acceptVehicle/x"; Trong đó x là license Plate Number
        private string requestAddVehicleUrl = "https://ubercloneserver.herokuapp.com/staff/getRequestAddingVehicle";
        private string acceptVehicleUrl = "https://ubercloneserver.herokuapp.com/staff/acceptVehicle/";

        IList<Vehicle> vehicles = new List<Vehicle>();
        List<Vehicle> VehiclesListToView = new List<Vehicle> { };
        List<Vehicle> SelectedVehicles = new List<Vehicle> { };
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        //References: cach lay row cua ListView sau khi bam nut nam trong row do (https://csharpcanban.com/wpf-lay-gia-tri-cua-row-listview-khi-click-vao-button-tren-listview.html)
        private void DeleteListItem(object sender, RoutedEventArgs e)
        {
            var curItem = ((ListBoxItem)vehicleListView.ContainerFromElement((Button)sender));
            vehicleListView.SelectedItem = (ListBoxItem)curItem;
            MessageBox.Show($"Selected index = {vehicleListView.SelectedIndex}");
        }
        private void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;
        }

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
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(requestAddVehicleUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            vehicles = arr.ToObject<List<Vehicle>>();
            refreshViewSource(vehicles);
        }

        public AddVehicleManagement()
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

        //private void acceptVehicleBtn_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnApproveVehicle_Click(object sender, RoutedEventArgs e)
        {
            int index = vehicleListView.SelectedIndex;
            Vehicle temp = vehicles.ElementAt(index);
            //MessageBox.Show(temp.ToString());
            string tempUrl = acceptVehicleUrl + temp.licensePlateNum;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.PutRequestWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            //MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Accept vehicle " + temp.licensePlateNum))
            {
                MessageBox.Show("Approve vehicle successfully");
                vehicles.Remove(temp);
                refreshViewSource(vehicles);
            }
            else
            {
                MessageBox.Show("Approve vehicle successfully");
            }
        }
    }
}
