using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public static class AccountnTokenHelper
    {
        public static string expiredTokenString { get; private set; } = "Token has expired and this is new your token";
        public static string userName { get; set; }
        public static string accessToken { get; set; }  
    }
}
