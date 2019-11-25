using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models.ViewModels
{
    public class OrderViewModels
    {
        public List<Orders> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
