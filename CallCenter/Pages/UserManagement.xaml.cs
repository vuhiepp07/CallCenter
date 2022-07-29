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

        IList<User> users = new List<User>();
        List<User> UsersListToView = new List<User> { };
        List<User> SelectedUsers = new List<User> { };
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        private CollectionViewSource UserViewSource;

        private void refreshViewSource(IList<User> users)
        {
            UsersListToView = (List<User>)users;
            SelectedUsers = UsersListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            UserViewSource.Source = SelectedUsers;
            _currentPage = 1;
            _totalItems = users.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }
        public void getAndBindingUserData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsyncWithAccessToken(GetAllUserUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            users = arr.ToObject<List<User>>();
            refreshViewSource(users);
        }
        public UserManagement()
        {
            InitializeComponent();
            UserViewSource = (CollectionViewSource)FindResource(nameof(UserViewSource));
            getAndBindingUserData();
        }

        private void btnLatestTrips_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFrequentDes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextUserPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedUsers = UsersListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                UserViewSource.Source = SelectedUsers;
            }
        }


        private void PrevUserPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedUsers = UsersListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                UserViewSource.Source = SelectedUsers;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var userName = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            UserViewSource.Source = from user in users
                                    where user.username.ToLower() == userName.ToLower()
                                    select user;
            refreshViewSource((IList<User>)UserViewSource.Source);
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingUserData();
        }
    }
}
