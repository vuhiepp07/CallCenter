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
    /// Interaction logic for CreateRequest.xaml
    /// </summary>
    public partial class CreateRequest : Page
    {
        private string Map4DApiUrl = "https://api.map4d.vn/sdk/place/text-search?key=49504631a3ee3700ee08bdca573b00c5&text=";
        private string mapurl = "https://www.google.com/maps/dir/";
        private string Start, End;

        IList<Address> startPoints = new List<Address>();
        IList<Address> endPoints = new List<Address>();

        public CreateRequest()
        {
            InitializeComponent();
        }


        private void StartCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Start = StartCBox.SelectedItem.ToString();
        }

        private void EndAddrbtn_Click(object sender, RoutedEventArgs e)
        {
            string endaddress = DestinationCBox.Text;
            string temp = Map4DApiUrl + endaddress;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetUserDriverAsync(temp);
            //MessageBox.Show(content);
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["result"];
            endPoints = arr.ToObject<List<Address>>();
            //MessageBox.Show(startPoints.ToString());
            DestinationCBox.ItemsSource = endPoints;
        }

        private void StartAddrbtn_Click(object sender, RoutedEventArgs e)
        {
            string startaddress = StartCBox.Text;
            string temp = Map4DApiUrl + startaddress;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetUserDriverAsync(temp);
            //MessageBox.Show(content);
            JObject o = JObject.Parse(content);
            JArray arr = (JArray)o["result"];
            startPoints = arr.ToObject<List<Address>>();
            //MessageBox.Show(startPoints.ToString());
            StartCBox.ItemsSource = startPoints;
        }

        private void sendRequestBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void viewRouteBtn_Click(object sender, RoutedEventArgs e)
        {
            Start = Start.Replace(" ", "+");
            End = End.Replace(" ", "+");
            mapurl += Start;
            mapurl += @"/";
            mapurl += End;
            Ggmap.Source = new Uri(mapurl);
            
        }

        private void DestinationCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            End = DestinationCBox.SelectedItem.ToString();
        }
    }
}

