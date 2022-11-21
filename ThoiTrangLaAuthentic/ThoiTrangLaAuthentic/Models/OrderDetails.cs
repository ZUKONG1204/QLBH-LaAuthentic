using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable
namespace ThoiTrangLaAuthentic.Models
{
    public partial class OrderDetails
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? OrderNumber { get; set; }
        public int? Amount { get; set; }
        public int? Discount { get; set; }
        public int? TotalMoney { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Price { get; set; }

        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}
