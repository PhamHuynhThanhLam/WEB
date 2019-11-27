using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models
{
    public class Stores
    {
        public int ID { get; set; }
        public int ImportDetailID { get; set; }

        [ForeignKey("ImportDetailID")]
        public virtual ImportDetails ImportDetails { get; set; }
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Products Products { get; set; }
    }
}
