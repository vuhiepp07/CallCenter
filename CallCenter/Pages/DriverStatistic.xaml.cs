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
    /// Interaction logic for DriverStatistic.xaml
    /// </summary>
    public partial class DriverStatistic : Page
    {
        private double[] userReviewNumOfStar;
        private double numofRequestRejected;
        private double numofRequestAccepted;
        private double[] income;
        private readonly double[] months = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        public double calAverageStar(double[] arr)
        {
            double result = 0;
            result += arr[0];
            result += (arr[1] * 2);
            result += (arr[2] * 3);
            result += (arr[3] * 4);
            result += (arr[4] * 5);

            result /= arr.Sum();

            return result;
        }

        public void drawIncomeChart()
        {
            income = new double[] { 24, 14, 16, 6, 31, 24, 7, 17, 27, 25, 19, 18 };

            // ignore the "real" X values and plot data at consecutive X values (0, 1, 2, 3...)
            double[] positions = DataGen.Consecutive(months.Length);
            WpfPlot3.Plot.AddScatter(positions, income);

            // then define tick labels based on "real" X values, rotate them, and give them extra space
            string[] labels = months.Select(x => x.ToString()).ToArray();
            WpfPlot3.Plot.XAxis.ManualTickPositions(positions, labels);
            WpfPlot3.Plot.XAxis.TickLabelStyle(rotation: 0);
            WpfPlot3.Plot.XAxis.SetSizeLimit(min: 50); // extra space for rotated ticks

            // apply axis labels, trigging a layout reset
            WpfPlot3.Plot.Title("Income report");
            WpfPlot3.Plot.YLabel("Millions (VND)");
            WpfPlot3.Plot.XLabel("Month");
        }
        public DriverStatistic()
        {
            InitializeComponent();
            drawIncomeChart();

            userReviewNumOfStar = new double[] { 778, 43, 283, 76, 184 };
            string[] labels = { "1 star", "2 star", "3 star", "4 star", "5 star" };
            var pie = WpfPlot1.Plot.AddPie(userReviewNumOfStar);
            pie.SliceLabels = labels;
            pie.ShowPercentages = true;
            WpfPlot1.Plot.Legend(location: Alignment.LowerLeft);
            //WpfPlot1.Plot.Title("Customer review");

            numofRequestRejected = 29;
            numofRequestAccepted = 89;
            string[] labels2 = { "Request rejected", "Request accepted" };
            var pie2 = WpfPlot2.Plot.AddPie(new double[] { numofRequestAccepted, numofRequestRejected });
            pie2.SliceLabels = labels2;
            pie2.ShowPercentages = true;
            WpfPlot2.Plot.Legend(location: Alignment.LowerLeft);
            //WpfPlot2.Plot.Title("Trip accept");

            RatingBar.Value = (int)calAverageStar(userReviewNumOfStar);
        }
    }
}
