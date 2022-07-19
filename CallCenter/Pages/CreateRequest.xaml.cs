using CallCenter.Models;
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
    /// <summary>
    /// Interaction logic for CreateRequest.xaml
    /// </summary>
    public partial class CreateRequest : Page
    {
        private string mapViewPlaceUrl = "https://www.google.com/maps/search/";
        private string Map4DApiUrl = "https://api.map4d.vn/sdk/place/text-search?key=49504631a3ee3700ee08bdca573b00c5&text=";
        private string mapRouteUrl = "https://www.google.com/maps/dir/";
        private string bookingUrl = "https://ubercloneserver.herokuapp.com/staff/booking";
        private string confirmBookingUrl = "https://ubercloneserver.herokuapp.com/staff/confirmBooking/";
        private string Start, End;
        //private string Map4DApiRouting = "http://api.map4d.vn/sdk/route?key=9535db74e2210bbdf73a8ce3c4fb03be&origin={origin}&destination={destination}&mode=motorcycle";
        JObject startObj, endObj, requestObj;


        IList<Address> startPoints = new List<Address>();
        IList<Address> endPoints = new List<Address>();
        List<string> vehicleList = new List<string>
        {
            "MOTORBIKE", "CAR4S", "CAR7S", "CAR16S"
        };

        public CreateRequest()
        {
            Start = "";
            End = "";
            InitializeComponent();
            VehicleCBox.ItemsSource = vehicleList;
        }


        private void StartCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Start = StartCBox.SelectedItem.ToString();
            if (End != "")
            {
                string tempStart = Start.Replace(" ", "+");
                string tempEnd = End.Replace(" ", "+");
                mapRouteUrl += tempStart;
                mapRouteUrl += @"/";
                mapRouteUrl += tempEnd;
                Ggmap.Source = new Uri(mapRouteUrl);
            }
            else
            {
                string temp = Start.Replace(" ", "+");
                Ggmap.Source = new Uri(mapViewPlaceUrl + temp);
            }
        }

        private void EndAddrbtn_Click(object sender, RoutedEventArgs e)
        {
            string endaddress = DestinationCBox.Text;
            string temp = Map4DApiUrl + endaddress;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsync(temp);
            //MessageBox.Show(content);
            endObj = JObject.Parse(content);
            JArray arr = (JArray)endObj["result"];
            endPoints = arr.ToObject<List<Address>>();
            //MessageBox.Show(startPoints.ToString());
            DestinationCBox.ItemsSource = endPoints;
            DestinationCBox.IsDropDownOpen = true;
        }

        private void StartAddrbtn_Click(object sender, RoutedEventArgs e)
        {
            string startaddress = StartCBox.Text;
            string temp = Map4DApiUrl + startaddress;
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsync(temp);
            //MessageBox.Show(content);
            startObj = JObject.Parse(content);
            JArray arr = (JArray)startObj["result"];
            startPoints = arr.ToObject<List<Address>>();
            //MessageBox.Show(startPoints.ToString());
            StartCBox.ItemsSource = startPoints;
            StartCBox.IsDropDownOpen = true;
        }

        private void sendRequestBtn_Click(object sender, RoutedEventArgs e)
        {
            int startIndex = StartCBox.SelectedIndex;
            int endIndex = DestinationCBox.SelectedIndex;
            string origin = (string)startObj["result"][startIndex]["location"]["lat"] + "," + (string)startObj["result"][startIndex]["location"]["lng"];
            string destination = (string)endObj["result"][startIndex]["location"]["lat"] + "," + (string)endObj["result"][startIndex]["location"]["lng"];
            //MessageBox.Show(origin + " ----" + destination);
            string map4DRoutingUrl = $"http://api.map4d.vn/sdk/route?key=9535db74e2210bbdf73a8ce3c4fb03be&origin={origin}&destination={destination}&mode=motorcycle";
            HttpRequest httpRequest = new HttpRequest();
            var content = httpRequest.GetDataFromUrlAsync(map4DRoutingUrl);
            //MessageBox.Show(content);
            requestObj = JObject.Parse(content);

            point a = new point()
            {
                address = StartCBox.Text,
                longitude = (string)startObj["result"][startIndex]["location"]["lng"],
                latitude = (string)startObj["result"][startIndex]["location"]["lat"]
            };

            point b = new point()
            {
                address = DestinationCBox.Text,
                longitude = (string)endObj["result"][startIndex]["location"]["lng"],
                latitude = (string)endObj["result"][startIndex]["location"]["lat"]
            };

            //MessageBox.Show(requestObj["result"]["routes"][0]["legs"].ToString());

            string dis = (string)requestObj["result"]["routes"][0]["legs"][0]["distance"]["value"];
            Request request = new Request()
            {
                startAddress = a,
                destination = b,
                timeSecond = (string)requestObj["result"]["routes"][0]["legs"][0]["duration"]["value"],
                phoneNumber = PhoneTextBox.Text,
                createdTime = DateTime.Now,
                distance = (double.Parse(dis)/1000.0).ToString(),
                vehicleType = VehicleCBox.Text,
                note = userNoteTextBox.Text
            };
            string json = JsonConvert.SerializeObject(request);
            httpRequest = new HttpRequest();
            string responseContent = httpRequest.PutAsyncJson(bookingUrl, json);
            //MessageBox.Show(responseContent);
            JObject objTemp = JObject.Parse(responseContent);
            string status = (string)objTemp["status"];
            string priceTemp = objTemp["data"]["price"].ToString();
            int price = (int)double.Parse(priceTemp);
            string message = (string)objTemp["message"];
            if (status.Equals("True") && message.Equals("Request and get price successfully"))
            {
                MessageBoxResult result =  MessageBox.Show($"The customer trip price is {price}VND, does him/her agree?", "User agreement", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes)
                {
                    string temp = confirmBookingUrl + (string)objTemp["data"]["requestId"];

                    httpRequest = new HttpRequest();
                    responseContent = httpRequest.PutRequest(temp);
                    //MessageBox.Show(responseContent);
                    objTemp = JObject.Parse(responseContent);
                    status = (string)objTemp["status"];
                    message = (string)objTemp["message"];
                    if(status.Equals("True"))
                    {
                        MessageBox.Show("Booking success");
                    }
                    else
                    {
                        MessageBox.Show("Some error happened, please try again");
                    }
                    this.NavigationService.Navigate(new Uri("/Pages/RequestManagement.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Request canceled");
                    this.NavigationService.Navigate(new Uri("/Pages/RequestManagement.xaml", UriKind.Relative));
                }
            }
        }

        //private void viewRouteBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    Start = Start.Replace(" ", "+");
        //    End = End.Replace(" ", "+");
        //    mapurl += Start;
        //    mapurl += @"/";
        //    mapurl += End;
        //    Ggmap.Source = new Uri(mapurl);
            
        //}

        private void DestinationCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            End = DestinationCBox.SelectedItem.ToString();

            if (Start !="")
            {
                string tempStart = Start.Replace(" ", "+");
                string tempEnd = End.Replace(" ", "+");
                mapRouteUrl += tempStart;
                mapRouteUrl += @"/";
                mapRouteUrl += tempEnd;
                Ggmap.Source = new Uri(mapRouteUrl);
            }
            else
            {
                string temp = End.Replace(" ", "+");
                Ggmap.Source = new Uri(mapViewPlaceUrl + temp);
            }
        }
    }
}

