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
    /// Interaction logic for TripManagement.xaml
    /// </summary>
    public partial class TripManagement : Page
    {
        private const string GetAllTripUrl = "https://ubercloneserver.herokuapp.com/staff/getAllTrip";
        private const string tripTrackingUrl = "";
        private PagingHelper<Trip> pagingHelper;

        IList<Trip> trips = new List<Trip>();


        private CollectionViewSource TripViewSource;

        private void refreshViewSource()
        {
            pagingHelper = new PagingHelper<Trip>(trips);
            TripViewSource.Source = pagingHelper.refreshView();
            PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
        }

        public void getAndBindingTripData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(GetAllTripUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            trips = arr.ToObject<List<Trip>>();
            refreshViewSource();
        }
        public TripManagement()
        {
            InitializeComponent();
            TripViewSource = (CollectionViewSource)FindResource(nameof(TripViewSource));
            getAndBindingTripData();
        }

        public TripManagement(List<Trip> tripReceived)
        {
            InitializeComponent();
            TripViewSource = (CollectionViewSource)FindResource(nameof(TripViewSource));
            this.trips = tripReceived;
            refreshViewSource();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var tripID = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            //TripViewSource.Source = from trip in trips
            //                        where trip.tripID.ToLower() == tripID.ToLower()
            //                        select trip;
            refreshViewSource();
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingTripData();
        }

        private void NextTripPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage < pagingHelper._totalPages)
            {
                TripViewSource.Source = pagingHelper.nextPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void PrevTripPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage > 1)
            {
                TripViewSource.Source = pagingHelper.prevPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void btnTracking_Click(object sender, RoutedEventArgs e)
        {
            Trip temp = (Trip)tripListView.SelectedItem;
            if (temp.status.Equals("Completed"))
            {

            }
            else
            {
                MessageBox.Show("This trip has reach its destination");
            }
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(GetAllTripUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Delete staff successfully"))
            {
                //JArray arr = (JArray)o["data"];
                //List<Trip> trips = arr.ToObject<List<Trip>>();
            }
            else
            {
                MessageBox.Show("Some error occured");
            }
        }
    }
}
