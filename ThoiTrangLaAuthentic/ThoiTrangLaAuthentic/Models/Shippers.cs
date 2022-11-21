using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace ThoiTrangLaAuthentic.Models
{
    public partial class Shippers
    {
        public int ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public DateTime? ShipDate { get; set; }
    }
}
