using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    internal class Request
    {
        public string requestID { get; set; }
        public string userName { get; set; }
        public string pickingAddress { get; set; }
        public string status { get; set; }
        public string createdTime { get; set; }
        public string typeOfVehicle { get; set; }
        public string arrivingAddress { get; set; }
    }
}
