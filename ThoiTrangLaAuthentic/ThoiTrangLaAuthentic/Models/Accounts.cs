using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable
namespace ThoiTrangLaAuthentic.Models
{
    public partial class Accounts
    {
        public int AccountId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool Active { get; set; }
        public string FullName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Roles Roles { get; set; }
    }
}
