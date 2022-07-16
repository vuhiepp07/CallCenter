using CallCenter.Models;
using CallCenter.Pages;
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
using System.Windows.Shapes;

namespace CallCenter.Windows
{
    /// <summary>
    /// Interaction logic for DiscountInputWindow.xaml
    /// </summary>
    public partial class DiscountInputWindow : Window
    {
        Discount resultAfterEdit;
        public DataTransferDelegate dataTransferDelegate;
        public DiscountInputWindow()
        {
            InitializeComponent();
        }

        public DiscountInputWindow(DataTransferDelegate del)
        {
            InitializeComponent();
            dataTransferDelegate = del;
            resultAfterEdit = new Discount();
        }

        public DiscountInputWindow(DataTransferDelegate del, Discount temp)
        {
            InitializeComponent();
            dataTransferDelegate = del;
            discountName.Text = temp.discountName;
            discountPercent.Text = temp.discountPercent.ToString();
            discountQuantity.Text = temp.quantity.ToString();
            startDatePicker.Text = temp.startDate.ToString();
            endDatePicker.Text = temp.endDate.ToString();
            resultAfterEdit = new Discount(temp);
        }

        private void addDiscountBtn_Click(object sender, RoutedEventArgs e)
        {
            resultAfterEdit.discountPercent = float.Parse(discountPercent.Text);
            resultAfterEdit.discountName = discountName.Text;
            resultAfterEdit.startDate = startDatePicker.Text;
            resultAfterEdit.endDate = endDatePicker.Text;
            resultAfterEdit.quantity = discountQuantity.Text;
            dataTransferDelegate?.Invoke(resultAfterEdit);
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            resultAfterEdit.discountPercent = -99.0;
            dataTransferDelegate?.Invoke(resultAfterEdit);
            this.Close();
        }
    }
}
