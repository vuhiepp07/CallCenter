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
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Page
    {
        private string GetAllUserUrl = "https://ubercloneserver.herokuapp.com/staff/getAllUser";
        private string Get5LatestTripUrl = "https://ubercloneserver.herokuapp.com/staff/get5latestTripForUser/";
        private string GetMostFrequenceDesUrl = "https://ubercloneserver.herokuapp.com/staff/get5mostDestination/";
        private PagingHelper<User> pagingHelper;
        IList<User> users = new List<User>();

        private CollectionViewSource UserViewSource;

        private void refreshViewSource()
        {
            pagingHelper = new PagingHelper<User>(users);
            UserViewSource.Source = pagingHelper.refreshView();
            PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
        }
        public void getAndBindingUserData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(GetAllUserUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            users = arr.ToObject<List<User>>();
            refreshViewSource();
        }
        public UserManagement()
        {
            InitializeComponent();
            UserViewSource = (CollectionViewSource)FindResource(nameof(UserViewSource));
            getAndBindingUserData();
        }

        private void btnLatestTrips_Click(object sender, RoutedEventArgs e)
        {
            User temp = (User)userListView.SelectedItem;
            string tempUrl = Get5LatestTripUrl + temp.userId;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Get 5 latest trip"))
            {
                JArray arr = (JArray)objTemp["data"];
                List<Trip> trips = arr.ToObject<List<Trip>>();
                if (trips.Count == 0)
                {
                    MessageBox.Show("This user did not have any trip");
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

        private void btnFrequentDes_Click(object sender, RoutedEventArgs e)
        {
            User temp = (User)userListView.SelectedItem;
            string tempUrl = GetMostFrequenceDesUrl + temp.userId;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            //if (status.Equals("True") && message.Equals("Get 5 latest trip"))
            //{
            //    JArray arr = (JArray)objTemp["data"];
            //    List<Trip> trips = arr.ToObject<List<Trip>>();
            //    if (trips.Count == 0)
            //    {
            //        MessageBox.Show("This user did not have any trip");
            //    }
            //    else
            //    {
            //        this.NavigationService.Navigate(new TripManagement(trips));
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Some error occured");
            //}
        }

        private void NextUserPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage < pagingHelper._totalPages)
            {
                UserViewSource.Source = pagingHelper.nextPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }


        private void PrevUserPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage > 1)
            {
                UserViewSource.Source = pagingHelper.prevPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var userName = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            UserViewSource.Source = from user in users
                                    where user.username.ToLower() == userName.ToLower()
                                    select user;
            refreshViewSource();
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingUserData();
        }
    }
}
