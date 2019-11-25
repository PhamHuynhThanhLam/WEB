using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models.ViewModels
{
    public class ProductViewModels
    {
        public Products Products { get; set; }
        public IEnumerable<Merchants> Merchants { get; set; }
    }
}
