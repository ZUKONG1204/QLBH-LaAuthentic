﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
#nullable disable
namespace ThoiTrangLaAuthentic.Models
{
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Phải nhập danh mục sản phẩm")]
        public int? CatId { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public string Thumb { get; set; }
        public string Video { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool BestSellers { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        [Required(ErrorMessage = ("Phải nhập số lượng"))]
        public int? UnitsInStock { get; set; }

        public virtual Categories Cat { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
