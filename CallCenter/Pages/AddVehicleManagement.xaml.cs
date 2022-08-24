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
        private PagingHelper<Vehicle> pagingHelper;
        IList<Vehicle> vehicles = new List<Vehicle>();

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

        private void refreshViewSource()
        {
            pagingHelper = new PagingHelper<Vehicle>(vehicles);
            VehicleViewSource.Source = pagingHelper.refreshView();
            PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
        }
        public void getAndBindingVehicleData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlWithAccessToken(requestAddVehicleUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            vehicles = arr.ToObject<List<Vehicle>>();
            refreshViewSource();
        }

        public AddVehicleManagement()
        {
            InitializeComponent();
            VehicleViewSource = (CollectionViewSource)FindResource(nameof(VehicleViewSource));
            getAndBindingVehicleData();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var licensePlateNum = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            VehicleViewSource.Source = from vehicle in vehicles
                                       where vehicle.licensePlateNum.ToLower() == licensePlateNum.ToLower()
                                       select vehicle;
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingVehicleData();
        }

        private void PrevVehiclePageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage > 1)
            {
                VehicleViewSource.Source = pagingHelper.prevPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void NextVehiclePageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage < pagingHelper._totalPages)
            {
                VehicleViewSource.Source = pagingHelper.nextPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
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
                refreshViewSource();
            }
            else
            {
                MessageBox.Show("Approve vehicle successfully");
            }
        }
    }
}
