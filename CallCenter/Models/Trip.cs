using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    internal class Trip
    {
        public string tripID { get; set; }
        public string pickingAddress { get; set; }
        public string arrivingAddress { get; set; }
        public string status { get; set; }
        public string departureTime { get; set; }
        public string arriveTime { get; set; }
        public string distance { get; set; }
        public string customerBookTime { get; set; }
        public string totalPrice { get; set; }
        public string discountID { get; set; }
        public string estimateTime { get; set; }
        public string paymentMethod { get; set; }
        public string customerName { get; set; }
        public string driverName { get; set; }
    }
}
