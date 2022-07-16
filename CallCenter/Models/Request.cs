using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    internal class Request
    {
        public Address startAddress { get; set; }
        public Address destination { get; set; }
        public string timeSecond { get; set; }
        public string phoneNumber { get; set; }
        public DateTime createdTime { get; set; }   
        public string distance { get; set; }
        public string vehicleType { get; set; }
        public string note { get; set; }

    }
}
