using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThoiTrangLaAuthentic.Models;

namespace ThoiTrangLaAuthentic.ModelViews
{
    public class XemDonHang
    {
        public Orders DonHang { get; set; }
        public List<OrderDetails> ChiTietDonHang { get; set; }
    }
}
