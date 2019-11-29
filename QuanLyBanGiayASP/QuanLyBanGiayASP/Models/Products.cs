using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models
{
    public class Products
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }

        public bool Available { get; set; }

        public string Image { get; set; }
        public string Description { get; set; }

        public int InStock { get; set; }

        [Display(Name = "Merchant")]
        public int MerchantId { get; set; }

        [ForeignKey("MerchantId")]
        public virtual Merchants Merchants { get; set; }

        [Display(Name = "Brand")]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brands Brands { get; set; }
       
    }
}
