using CallCenter.Models;
using CallCenter.Windows;
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
    /// Interaction logic for DiscountManagement.xaml
    /// </summary>
    public partial class DiscountManagement : Page
    {
        private const string GetAllDiscountUrl = "https://ubercloneserver.herokuapp.com/staff/getAllDiscount";
        private const string deleteDiscountUrl = "https://ubercloneserver.herokuapp.com/staff/deleteDiscount/";

        IList<Discount> discounts = new List<Discount>();
        List<Discount> DiscountsListToView = new List<Discount> { };
        List<Discount> SelectedDiscounts = new List<Discount> { };
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        private CollectionViewSource DiscountViewSource;

        public void getAndBindingDiscountData()
        {
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsync(GetAllDiscountUrl);
            MessageBox.Show(content.ToString());
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["data"];
            discounts = arr.ToObject<List<Discount>>();
            refreshViewSource(discounts);
        }
        public DiscountManagement()
        {
            InitializeComponent();
            DiscountViewSource = (CollectionViewSource)FindResource(nameof(DiscountViewSource));
            getAndBindingDiscountData();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var discountID = SearchField.Text;
            SearchField.Text = "";
            DiscountViewSource.Source = from discount in discounts
                                        where discount.discountId == discountID
                                        select discount;
            refreshViewSource((IList<Discount>)DiscountViewSource.Source);
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            getAndBindingDiscountData();    
        }

        private void PrevDiscountPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedDiscounts = DiscountsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                DiscountViewSource.Source = SelectedDiscounts;
            }
        }

        private void NextDiscountPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
                SelectedDiscounts = DiscountsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                DiscountViewSource.Source = SelectedDiscounts;
            }
        }

        private void addDiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            var addDiscountWindow = new AddDiscount();
            addDiscountWindow.ShowDialog();
        }

        private void refreshViewSource(IList<Discount> discounts)
        {
            DiscountsListToView = (List<Discount>)discounts;
            SelectedDiscounts = DiscountsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            DiscountViewSource.Source = SelectedDiscounts;
            _currentPage = 1;
            _totalItems = discounts.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }

        private void deleteDiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            Discount temp = (Discount)discountListView.SelectedItem;
            string tempUrl = deleteDiscountUrl + temp.discountId;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.DeleteDataByUrl(tempUrl);
            //MessageBox.Show(content);
            JObject objTemp = JObject.Parse(content);
            string status = (string)objTemp["status"];
            string message = (string)objTemp["message"];
            if(status.Equals("True") && message.Equals("Delete discount successfully")){
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
    }
}
