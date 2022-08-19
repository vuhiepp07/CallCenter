using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class Address
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }    
        public override string ToString()
        {
            return $"{name} - {address}";
        }
    }
}
