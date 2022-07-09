using ScottPlot;
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
    /// Interaction logic for RequestStatistic.xaml
    /// </summary>
    public partial class RequestStatistic : Page
    {
        double[] PickingValues1;
        double[] PickingValues2;

        double[] DestinationValues1;
        double[] DestinationValues2;
        private readonly string[] labels1 = { "District 1", "District 2", "District 3", "District 4", "District 5", "District 6", "District 7", "District 8", "District 9", "District 10", "District 11", "District 12" };
        private readonly string[] labels2 = { "Binh Tan", "Binh Thanh", "Go Vap", "Phu Nhuan", "Tan Binh", "Tan Phu", "Thu Duc", "Binh Chanh", "Can Gio", "Cu Chi", "Hoc Mon", "Nha Be" };
        private readonly string[] seriesNames = { "Request start point", "Request destination" };
        private readonly double[] err = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[][] valueBySeries1;
        private double[][] valueBySeries2;
        private double[][] errorBySeries;
        int _currentPage = 0;
        int _totalPages = 0;
        public RequestStatistic()
        {
            InitializeComponent();
            PickingValues1 = new double[] { 256, 322, 567, 222, 444, 555, 823, 312, 321, 328, 432, 875 };
            DestinationValues1 = new double[] { 321, 124, 525, 312, 666, 888, 555, 111, 999, 333, 111, 777 };

            PickingValues2 = new double[] { 543, 432, 432, 411, 455, 678, 542, 654, 641, 521, 531, 523 };
            DestinationValues2 = new double[] { 222, 111, 654, 456, 765, 542, 542, 432, 532, 444, 532, 542 };

            valueBySeries1 = new double[][] { PickingValues1, DestinationValues1 };
            valueBySeries2 = new double[][] { PickingValues2, DestinationValues2 };
            errorBySeries = new double[][] { err, err };

            WpfPlot1.Plot.AddBarGroups(labels1, seriesNames, valueBySeries1, errorBySeries);

            // add a legend to display each labeled bar plot
            WpfPlot1.Plot.Legend(location: Alignment.UpperRight);

            // adjust axis limits so there is no padding below the bar graph
            WpfPlot1.Plot.SetAxisLimits(yMin: 0, yMax: 1100);
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                WpfPlot1.Plot.AddBarGroups(labels2, seriesNames, valueBySeries2, errorBySeries);
            }
        }

        private void PrevPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                WpfPlot1.Plot.AddBarGroups(labels1, seriesNames, valueBySeries1, errorBySeries);
            }
        }
    }
}
