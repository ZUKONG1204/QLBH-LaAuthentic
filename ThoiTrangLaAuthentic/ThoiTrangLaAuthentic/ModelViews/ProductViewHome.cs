using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThoiTrangLaAuthentic.Models;

namespace ThoiTrangLaAuthentic.ModelViews
{
    public class ProductViewHome
    {
        public Categories category { get; set; }
        public List<Products> lsProducts { get; set; }
    }
}
