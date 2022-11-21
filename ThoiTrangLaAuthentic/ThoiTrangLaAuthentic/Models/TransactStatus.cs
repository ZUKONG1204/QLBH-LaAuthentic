using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable
namespace ThoiTrangLaAuthentic.Models
{
    public class TransactStatus
    {
        public TransactStatus()
        {
            Orders = new HashSet<Orders>();
        }

        public int TransactStatusId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
