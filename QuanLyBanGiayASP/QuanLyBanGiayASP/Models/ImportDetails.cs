using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanGiayASP.Models
{
    public class ImportDetails
    {
        public int ID { get; set; }
        public DateTime DateImport { get; set; }
        public int Total { get; set; }

    }
}
