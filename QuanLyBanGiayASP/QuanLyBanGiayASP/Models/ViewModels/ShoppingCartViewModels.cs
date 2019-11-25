using QuanLyBanGiayASP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models.ViewModels
{
    public class ShoppingCartViewModels
    {
        public List<Products> Products { get; set; }
        public Orders Orders { get; set; }
        //public Customers Customers { get; set; }     
    }
}
