using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable
namespace ThoiTrangLaAuthentic.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? LocationId { get; set; }
        public int? District { get; set; }
        public int? Ward { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
