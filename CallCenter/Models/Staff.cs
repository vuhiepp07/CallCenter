using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class Staff
    {
        public string id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string type { get; set; }
        
        public Staff(){

        }   

        public Staff(Staff temp){
            id = temp.id;
            username = temp.username;
            fullname = temp.fullname;
            phone = temp.phone;
            address = temp.address;
            email = temp.email;
            gender = temp.gender;
            type = temp.type;
        } 

    }

}
