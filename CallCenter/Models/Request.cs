using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    internal class Request
    {
        public string requestId { get; set; }
        public string userId { get; set; }
        public point startAddress { get; set; }
        public point destination { get; set; }
        public string timeSecond { get; set; }
        public string phoneNumber { get; set; }
        public DateTime createdTime { get; set; }   
        public string distance { get; set; }
        public string vehicleType { get; set; }
        public string note { get; set; }
        public string status { get; set; }
    }
}
