using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models
{
    public class ProductStores
    {
        public int ID { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Products Products { get; set; }
        public int StoreID { get; set; }

        [ForeignKey("StoreID")]
        public virtual Stores Stores { get; set; }
    }
}
