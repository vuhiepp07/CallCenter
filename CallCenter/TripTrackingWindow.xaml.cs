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

namespace CallCenter
{
    /// <summary>
    /// Interaction logic for TripTrackingWindow.xaml
    /// </summary>
    public partial class TripTrackingWindow : Window
    {
        private string accessToken;
        public TripTrackingWindow()
        { 
            InitializeComponent();
        }

        public TripTrackingWindow(string token)
        {
            accessToken = token;
            InitializeComponent();
        }
    }
}
