using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models
{
    public class Orders
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public DateTime Time { get; set; }
        public bool isConfirmed { get; set; }

        [Display(Name = "Sales Person")]
        public string SalesPersonId { get; set; }

        [ForeignKey("SalesPersonId")]
        public virtual ApplicationUser SalerPerson { get; set; }

        [Display(Name = "CustomerID")]
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customers Customers { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }

      
    }
}
