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
    public delegate void DataTransferDelegate(Discount data);
    /// <summary>
    /// Interaction logic for DiscountManagement.xaml
    /// </summary>
    public partial class DiscountManagement : Page
    {
        private const string GetAllDiscountUrl = "https://ubercloneserver.herokuapp.com/staff/getAllDiscount";
        private const string deleteDiscountUrl = "https://ubercloneserver.herokuapp.com/staff/deleteDiscount/";
        private const string editDiscountUrl = "https://ubercloneserver.herokuapp.com/staff/editDiscount";
        private string addDiscountURL = "https://ubercloneserver.herokuapp.com/staff/addDiscount";
        private Discount unEdited;
        private PagingHelper<Discount> pagingHelper;

        public DataTransferDelegate del;
        Boolean addDiscountflag, editDiscountflag;

        IList<Discount> discounts = new List<Discount>();

        private CollectionViewSource DiscountViewSource;

        private void refreshViewSource(IList<Discount> discounts)
        {
            pagingHelper = new PagingHelper<Discount>(discounts);
            DiscountViewSource.Source = pagingHelper.refreshView();
            PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
        }
        public void getAndBindingDiscountData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlWithAccessToken(GetAllDiscountUrl, AccountnTokenHelper.accessToken);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            discounts = arr.ToObject<List<Discount>>();
            refreshViewSource(discounts);
        }
        public DiscountManagement()
        {
            addDiscountflag = false;
            editDiscountflag = false;
            InitializeComponent();
            DiscountViewSource = (CollectionViewSource)FindResource(nameof(DiscountViewSource));
            getAndBindingDiscountData();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var discountName = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            DiscountViewSource.Source = from discount in discounts
                                       where discount.discountName.ToLower() == discountName.ToLower()
                                       select discount;
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingDiscountData();
        }

        private void PrevDiscountPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage > 1)
            {
                DiscountViewSource.Source = pagingHelper.prevPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }

        private void NextDiscountPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pagingHelper._currentPage < pagingHelper._totalPages)
            {
                DiscountViewSource.Source = pagingHelper.nextPage();
                PagesTextBlock.Text = $"{pagingHelper._currentPage}/{pagingHelper._totalPages}";
            }
        }


        private void deleteDiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            Discount temp = (Discount)discountListView.SelectedItem;
            string tempUrl = deleteDiscountUrl + temp.discountId;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.DeleteDataByUrlWithAccessToken(tempUrl, AccountnTokenHelper.accessToken);
            //MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Delete discount successfully"))
            {
                MessageBox.Show("Delete discount successfully");
                int index = discounts.IndexOf(temp);
                discounts.RemoveAt(index);
                refreshViewSource(discounts);
            }
            else
            {
                MessageBox.Show("Some error occured when delete this discount");
            }
        }

        private void editDiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            unEdited = (Discount)discountListView.SelectedItem;
            editDiscountflag = true;
            del += new DataTransferDelegate(passData);
            DiscountInputWindow discountInputWindow = new DiscountInputWindow(del, new Discount((Discount)discountListView.SelectedItem));
            discountInputWindow.Show();
        }

        private void addDiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            addDiscountflag = true;
            del += new DataTransferDelegate(passData);
            DiscountInputWindow discountInputWindow = new DiscountInputWindow(del);
            discountInputWindow.Show();
        }

        public void passData(Discount data)
        {
            Discount temp = new Discount(data);
            if (data.discountPercent == -99.0)
            {
                addDiscountflag = false;
                editDiscountflag = false;
                unEdited = null;
                del = null;
            }
            else if (addDiscountflag == true)
            {
                string json = JsonConvert.SerializeObject(temp);

                HttpRequest httpRequest = new HttpRequest();
                string responseContent = httpRequest.PostJsonWithAccessToken(addDiscountURL, json, AccountnTokenHelper.accessToken);
                //MessageBox.Show(responseContent);
                JObject objTemp = JObject.Parse(responseContent);
                string status = (string)objTemp["status"];
                string message = (string)objTemp["message"];
                if (status.Equals("True") && message.Equals("Add discount successfully"))
                {
                    MessageBox.Show("Add discount successfully");
                }
                addDiscountflag = false;
                getAndBindingDiscountData();
            }
            else if (editDiscountflag == true)
            {
                string json = JsonConvert.SerializeObject(temp);
                HttpRequest httpRequest = new HttpRequest();
                string responseContent = httpRequest.PostJsonWithAccessToken(editDiscountUrl, json, AccountnTokenHelper.accessToken);
                //MessageBox.Show(responseContent);

                JObject objTemp = JObject.Parse(responseContent);
                string status = (string)objTemp["status"];
                string message = (string)objTemp["message"];
                if (status.Equals("True") && message.Equals("Edit discount successfully"))
                {
                    MessageBox.Show("Edit discount successfully");
                }
                //MessageBox.Show(discounts.IndexOf(unEdited).ToString());
                discounts.RemoveAt(discounts.IndexOf(unEdited));
                unEdited = null;
                editDiscountflag = false;
                discounts.Add(temp);
                refreshViewSource(discounts);
            }
            del = null;
        }
    }
}
