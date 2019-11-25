using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models
{
    public class Customers
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string Date { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
