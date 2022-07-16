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
    /// Interaction logic for StaffInputWindow.xaml
    /// </summary>
    public partial class StaffInputWindow : Window
    {
        Staff resultAfterEdit;
        public DataTransferDelegateStaff dataTransferDelegateStaff;
        public StaffInputWindow()
        {
            InitializeComponent();
        }

        public StaffInputWindow(DataTransferDelegateStaff del)
        {
            InitializeComponent();
            dataTransferDelegateStaff = del;
            resultAfterEdit = new Staff();
        }

        private void addStaffBtn_Click(object sender, RoutedEventArgs e)
        {
            //resultAfterEdit.discountPercent = float.Parse(discountPercent.Text);
            //resultAfterEdit.discountName = discountName.Text;
            //resultAfterEdit.startDate = startDatePicker.Text;
            //resultAfterEdit.endDate = endDatePicker.Text;
            //resultAfterEdit.quantity = discountQuantity.Text;
            //dataTransferDelegateStaff?.Invoke(resultAfterEdit);
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
