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
    /// Interaction logic for IncomeAndNumOfTripReport.xaml
    /// </summary>
    public partial class IncomeAndNumOfTripReport : Page
    {
        private double[] income;
        private double[] numOfTrips;
        private readonly double[] months = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        public void drawIncomeChart()
        {
            income = new double[] { 234, 124, 146, 666, 333, 294, 785, 746, 927, 485, 898, 784 };

            // ignore the "real" X values and plot data at consecutive X values (0, 1, 2, 3...)
            double[] positions = DataGen.Consecutive(months.Length);
            WpfPlot1.Plot.AddScatter(positions, income);

            // then define tick labels based on "real" X values, rotate them, and give them extra space
            string[] labels = months.Select(x => x.ToString()).ToArray();
            WpfPlot1.Plot.XAxis.ManualTickPositions(positions, labels);
            WpfPlot1.Plot.XAxis.TickLabelStyle(rotation: 0);
            WpfPlot1.Plot.XAxis.SetSizeLimit(min: 50); // extra space for rotated ticks

            // apply axis labels, trigging a layout reset
            WpfPlot1.Plot.Title("Income report");
            WpfPlot1.Plot.YLabel("Millions (VND)");
            WpfPlot1.Plot.XLabel("Month");
        }

        public void drawTripNumberChart()
        {
            numOfTrips = new double[] { 3344, 1111, 2222, 7777, 7233, 2355, 1233, 1234, 2123, 2155, 3311, 3245 };
            // ignore the "real" X values and plot data at consecutive X values (0, 1, 2, 3...)
            double[] positions = DataGen.Consecutive(months.Length);
            WpfPlot2.Plot.AddScatter(positions, numOfTrips);

            // then define tick labels based on "real" X values, rotate them, and give them extra space
            string[] labels = months.Select(x => x.ToString()).ToArray();
            WpfPlot2.Plot.XAxis.ManualTickPositions(positions, labels);
            WpfPlot2.Plot.XAxis.TickLabelStyle(rotation: 0);
            WpfPlot2.Plot.XAxis.SetSizeLimit(min: 50); // extra space for rotated ticks

            // apply axis labels, trigging a layout reset
            WpfPlot2.Plot.Title("Num of trips");
            WpfPlot2.Plot.YLabel("Trips");
            WpfPlot2.Plot.XLabel("Month");
        }
        public IncomeAndNumOfTripReport()
        {
            InitializeComponent();
            drawIncomeChart();
            drawTripNumberChart();
        }
    }
}
