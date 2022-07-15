using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    internal class Discount
    {
        public string discountId { get; set; }
        public string discountName { get; set; }
        public double discountPercent { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string quantity { get; set; }
    }
}
