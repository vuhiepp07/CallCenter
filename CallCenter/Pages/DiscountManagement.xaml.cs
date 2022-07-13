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
    /// Interaction logic for DiscountManagement.xaml
    /// </summary>
    public partial class DiscountManagement : Page
    {
        private const string GetAllDiscountUrl = "https://ubercloneserver.herokuapp.com/staff/getAllDiscount";

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
            DiscountsListToView = (List<Discount>)discounts;
            SelectedDiscounts = DiscountsListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            DiscountViewSource.Source = SelectedDiscounts;
            _currentPage = 1;
            _totalItems = discounts.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            PagesTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }
        public DiscountManagement()
        {
            InitializeComponent();
            DiscountViewSource = (CollectionViewSource)FindResource(nameof(DiscountViewSource));
            getAndBindingDiscountData();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var discountID = SearchField.Text.Trim().ToLower();
            SearchField.Text = "";
            DiscountViewSource.Source = from discount in discounts
                                    where discount.discountID.ToLower() == discountID.ToLower()
                                    select
                                    new
                                    {
                                        discountID = discount.discountID,
                                        name = discount.name,
                                        discountPercentage = discount.discountPercentage,
                                        startDate = discount.startDate,
                                        endDate = discount.endDate,
                                        quantity = discount.quantity,
                                    };
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
    }
}
