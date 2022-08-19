using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string id { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string homeAddress { get; set; }
        public string gender { get; set; }
    }
}
