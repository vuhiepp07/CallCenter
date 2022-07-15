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
        private const string GetAllTripUrl = "https://ubercloneserver.herokuapp.com/staff/getAllUser";

        IList<Trip> trips = new List<Trip>();
        List<Trip> TripsListToView = new List<Trip> { };
        List<Trip> SelectedTrips = new List<Trip> { };
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        private CollectionViewSource TripViewSource;

        private void refreshViewSource(IList<Trip> trips)
        {
            TripsListToView = (List<Trip>)trips;
            SelectedTrips = TripsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            TripViewSource.Source = SelectedTrips;
            _currentPage = 1;
            _totalItems = trips.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }

        public void getAndBindingTripData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsync(GetAllTripUrl);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            trips = arr.ToObject<List<Trip>>();
            refreshViewSource(trips);
        }
        public TripManagement()
        {
            InitializeComponent();
            TripViewSource = (CollectionViewSource)FindResource(nameof(TripViewSource));
            getAndBindingTripData();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var tripID = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            TripViewSource.Source = from trip in trips
                                    where trip.tripID.ToLower() == tripID.ToLower()
                                    select trip;
            refreshViewSource((IList<Trip>)TripViewSource.Source);
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingTripData();
        }

        private void NextTripPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedTrips = TripsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                TripViewSource.Source = SelectedTrips;
            }
        }

        private void PrevTripPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedTrips = TripsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                TripViewSource.Source = SelectedTrips;
            }
        }
    }
}
