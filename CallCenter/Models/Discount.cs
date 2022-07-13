using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    internal class Discount
    {
        public string discountID { get; set; }
        public string name { get; set; }
        public string discountPercentage { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string quantity { get; set; }
    }
}
