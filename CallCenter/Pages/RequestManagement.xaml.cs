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
    /// Interaction logic for RequestManagement.xaml
    /// </summary>
    public partial class RequestManagement : Page
    {
        string accessToken;
        private string cancelBookingUrl = "https://ubercloneserver.herokuapp.com/staff/cancelBooking/";
        private void refreshViewSource(IList<Request> requests)
        {
            RequestsListToView = (List<Request>)requests;
            SelectedRequests = RequestsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            RequestViewSource.Source = SelectedRequests;
            _currentPage = 1;
            _totalItems = requests.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }
        public void getAndBindingRequestData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsync(GetAllRequestUrl, accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            //requests = arr.ToObject<List<Request>>();
            refreshViewSource(requests);
        }

        public RequestManagement(string token)
        {
            InitializeComponent();
            accessToken = token;
            RequestViewSource = (CollectionViewSource)FindResource(nameof(RequestViewSource));
            getAndBindingRequestData();
        }
        public RequestManagement()
        {
            InitializeComponent();
            RequestViewSource = (CollectionViewSource)FindResource(nameof(RequestViewSource));
            getAndBindingRequestData();
        }

        private const string GetAllRequestUrl = "https://ubercloneserver.herokuapp.com/staff/getAllRequest";

        IList<Request> requests = new List<Request>();
        List<Request> RequestsListToView = new List<Request> { };
        List<Request> SelectedRequests = new List<Request> { };
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 13;

        private CollectionViewSource RequestViewSource;

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var requestID = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            //RequestViewSource.Source = from request in requests
            //                           where request.requestedID.ToLower() == requestID.ToLower()
            //                           select request;
            refreshViewSource((IList<Request>)RequestViewSource.Source);
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingRequestData();
        }

        private void NextRequestPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedRequests = RequestsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                RequestViewSource.Source = SelectedRequests;
            }
        }
        
        private void PrevRequestPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedRequests = RequestsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                RequestViewSource.Source = SelectedRequests;
            }
        }

        private void addRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Pages/CreateRequest.xaml", UriKind.Relative));
        }

        private void btnCancelRequest_Click(object sender, RoutedEventArgs e)
        {
            var unEdited = (Request)requestListView.SelectedItem;
            string selectedRequestId = unEdited.requestId;
            string tempUrl = cancelBookingUrl + selectedRequestId;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.PutRequest(tempUrl);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
        }
    }
}
