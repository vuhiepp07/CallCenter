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
        private PagingHelper<Vehicle> pagingHelper;
        IList<Vehicle> vehicles = new List<Vehicle>();

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
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(GetAllVehicleUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            vehicles = arr.ToObject<List<Vehicle>>();
            refreshViewSource();
        }

        public VehicleManagement()
        {
            InitializeComponent();
            VehicleViewSource = (CollectionViewSource)FindResource(nameof(VehicleViewSource));
            getAndBindingVehicleData();
        }

        public VehicleManagement(List<Vehicle> list)
        {
            InitializeComponent();
            VehicleViewSource = (CollectionViewSource)FindResource(nameof(VehicleViewSource));
            vehicles = list;
            ViewAddVehicleRequestListbtn.IsEnabled = false;
            refreshViewSource();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var plateNum = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            VehicleViewSource.Source = from vehicle in vehicles
                                       where vehicle.licensePlateNum.ToLower() == plateNum.ToLower()
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

        private void ViewAddVehicleRequestListbtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/AddVehicleManagement.xaml", UriKind.Relative));
        }
    }
}
