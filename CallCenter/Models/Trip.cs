using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class Trip
    {
        public string tripId { get; set; }
        public point startAddress { get; set; }
        public point destination { get; set; }
        public string createdTime { get; set; }
        public vehiclePrice vehicleAndPrice { get; set; }
        public distanceTime distanceAndTime { get; set; }
        public string paymentType { get; set; }
        public string userId { get; set; }
        public string driverId { get; set; }
        public string status { get; set; }
    }
}
