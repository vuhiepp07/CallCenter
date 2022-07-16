using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class Discount
    {
        public string discountId { get; set; }
        public string discountName { get; set; }
        public double discountPercent { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string quantity { get; set; }

        public Discount(Discount temp)
        {
            discountId = temp.discountId;
            discountName = temp.discountName;
            discountPercent = temp.discountPercent;
            startDate = temp.startDate;
            endDate = temp.endDate;
            quantity = temp.quantity;
        }

        public Discount()
        {

        }

        public Discount(string discountId, string discountName, double discountPercent, string startDate, string endDate, string quantity)
        {
            this.discountId = discountId;
            this.discountName = discountName;
            this.discountPercent = discountPercent;
            this.startDate = startDate;
            this.endDate = endDate;
            this.quantity = quantity;
        }
    }
}
