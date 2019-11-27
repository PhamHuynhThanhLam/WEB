using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models.ViewModels
{
    public class StoreImportDetailViewModels
    {
        public Products Products { get; set; }
        public List<Products> ListProducts { get; set; }
        public ImportDetails ImportDetails { get; set; }
        public IEnumerable<Merchants> Merchants { get; set; }
    }
}
