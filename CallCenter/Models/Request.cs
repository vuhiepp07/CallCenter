using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class Request : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string requestId { get; set; }
        public string userId { get; set; }
        public point startAddress { get; set; }
        public point destination { get; set; }
        public string phoneNumber { get; set; }
        public string createdTime { get; set; }
        public vehiclePrice vehicleAndPrice { get; set; }
        public distanceTime distanceAndTime { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public string discountId { get; set; }

    }
}
