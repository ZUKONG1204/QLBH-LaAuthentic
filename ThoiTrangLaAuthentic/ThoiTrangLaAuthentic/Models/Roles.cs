using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace ThoiTrangLaAuthentic.Models
{
    public partial class Roles
    {
        public Roles()
        {
            Accounts = new HashSet<Accounts>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Accounts> Accounts { get; set; }
    }
}
