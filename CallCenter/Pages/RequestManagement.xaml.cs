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
        private string cancelBookingUrl = "https://ubercloneserver.herokuapp.com/staff/cancelBooking/";
        private const string GetAllRequestUrl = "https://ubercloneserver.herokuapp.com/staff/getAllRequest";

        IList<Request> requests = new List<Request>();

        private CollectionViewSource RequestViewSource;

        private PagingHelper<Request> pagingHelper;

        private void refreshViewSource()
        {
            pagingHelper = new PagingHelper<Request>(requests);
            RequestViewSource.Source = pagingHelper.refreshView();
            PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
        }
        public void getAndBindingRequestData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlWithAccessToken(GetAllRequestUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            requests = arr.ToObject<List<Request>>();
            requests = Enumerable.Reverse(requests).ToList();
            refreshViewSource();
        }
        public RequestManagement()
        {
            InitializeComponent();
            RequestViewSource = (CollectionViewSource)FindResource(nameof(RequestViewSource));
            getAndBindingRequestData();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var requestID = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            RequestViewSource.Source = from request in requests
                                       where request.requestId.ToLower() == requestID.ToLower()
                                       select request;
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingRequestData();
        }

        private void NextRequestPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage < pagingHelper._totalPages)
            {
                RequestViewSource.Source = pagingHelper.nextPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void PrevRequestPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage > 1)
            {
                RequestViewSource.Source = pagingHelper.prevPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
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
            var content = httpRequest.PutRequestWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Cancel booking successfully"))
            {
                MessageBox.Show("Cancel request successfully");
                unEdited.status = "Canceled";
            }
        }
    }
}
