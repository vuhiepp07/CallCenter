using CallCenter.Models;
using CallCenter.Windows;
using Newtonsoft.Json;
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
    public delegate void DataTransferDelegateStaff(bool result);
    /// <summary>
    /// Interaction logic for StaffManagement.xaml
    /// </summary>
    public partial class StaffManagement : Page
    {
        private string deleteStaffUrl = "https://ubercloneserver.herokuapp.com/staff/deleteStaff";
        private string getAllStaffUrl = "https://ubercloneserver.herokuapp.com/staff/getAllStaff";
        private PagingHelper<Staff> pagingHelper;
        public DataTransferDelegateStaff del;
        Boolean addStaffflag;

        IList<Staff> staffs = new List<Staff>();


        private CollectionViewSource StaffViewSource;

        public void getAndBindingStaffData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlWithAccessToken(getAllStaffUrl, AccountnTokenHelper.accessToken);
            //MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            staffs = arr.ToObject<List<Staff>>();
            //MessageBox.Show(arr.ToString());
            refreshViewSource();
        }

        public StaffManagement()
        {
            addStaffflag = false;
            InitializeComponent();
            StaffViewSource = (CollectionViewSource)FindResource(nameof(StaffViewSource));
            getAndBindingStaffData();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var staffId = SearchField.Text;
            SearchField.Text = "";
            StaffViewSource.Source = from staff in staffs
                                     where staff.id == staffId
                                     select staff;
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingStaffData();    
        }

        private void refreshViewSource()
        {
            pagingHelper = new PagingHelper<Staff>(staffs);
            StaffViewSource.Source = pagingHelper.refreshView();
            PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
        }

        private void PrevStaffPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage > 1)
            {
                StaffViewSource.Source = pagingHelper.prevPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void NextStaffPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage < pagingHelper._totalPages)
            {
                StaffViewSource.Source = pagingHelper.nextPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void addStaffBtn_Click(object sender, RoutedEventArgs e)
        {
            addStaffflag = true;
            del += new DataTransferDelegateStaff(passData);
            StaffInputWindow staffInputWindow = new StaffInputWindow(del);
            staffInputWindow.Show();
        }

        public void passData(bool result)
        {
            if(result == true)
            {
                getAndBindingStaffData();
            }
        }

        private void deleteStaffbtn_Click(object sender, RoutedEventArgs e)
        {
            Staff temp = (Staff)staffListView.SelectedItem;
            var temp2 = new { staffId= temp.id};
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.PostJsonWithAccessToken(deleteStaffUrl, JsonConvert.SerializeObject(temp2).ToString(), AccountnTokenHelper.accessToken);
            //MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Delete staff successfully"))
            {
                MessageBox.Show(message);
                int index = staffs.IndexOf(temp);
                staffs.RemoveAt(index);
                refreshViewSource();
            }
            else
            {
                MessageBox.Show("Some error occured when delete this staff");
            }
        }
    }
}
