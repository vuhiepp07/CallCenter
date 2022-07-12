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
        public void getAndBindingRequestData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetUserDriverAsync(GetAllDriverUrl);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            requests = arr.ToObject<List<Request>>();
            RequestsListToView = (List<Request>)requests;
            SelectedRequests = RequestsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            RequestViewSource.Source = SelectedRequests;
            _currentPage = 1;
            _totalItems = requests.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }
        public RequestManagement()
        {
            InitializeComponent();
            RequestViewSource = (CollectionViewSource)FindResource(nameof(RequestViewSource));
            getAndBindingRequestData();
        }

        private const string GetAllDriverUrl = "https://ubercloneserver.herokuapp.com/staff/getAllDriver";

        IList<Request> requests = new List<Request>();
        List<Request> RequestsListToView = new List<Request> { };
        List<Request> SelectedRequests = new List<Request> { };
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        private CollectionViewSource RequestViewSource;

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var requestID = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            //RequestViewSource.Source = from request in requests
            //                        where request.ID.ToLower() == requestID.ToLower()
            //                        select
            //                        new
            //                        {
            //                            id = request.driverId,
            //                            fullName = request.fullname,
            //                            status = request.status,
            //                            username = request.username,
            //                            phone = request.phone,
            //                            email = request.email,
            //                            address = request.address,
            //                            gender = request.gender,
            //                            currentLocation = request.currentLocation
            //                        };
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
    }
}
