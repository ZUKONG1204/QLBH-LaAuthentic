using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThoiTrangLaAuthentic.Models;

namespace ThoiTrangLaAuthentic.ModelViews
{
    public class CartItem
    {
        public Products products { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * products.Price.Value;
    }
}
