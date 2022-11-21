using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
#nullable disable
namespace ThoiTrangLaAuthentic.Models
{
    public partial class Locations
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public int? Parent { get; set; }
        public int? Levels { get; set; }
        public string Slug { get; set; }
        public string NameWithType { get; set; }
        public string Type { get; set; }
    }
}
