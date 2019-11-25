using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models
{
    public class OrderItems
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int Amount { get; set; }

        [ForeignKey("OrderID")]
        public virtual Orders Orders { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Products Products { get; set; }
    }
}
