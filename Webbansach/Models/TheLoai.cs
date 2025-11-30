using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webbansach.Models
{
    public class TheLoai
    {
        [Key] public int MaTheLoai { get; set; }
        [Required, StringLength(100)] public string TenTheLoai { get; set; }
        public virtual ICollection<Sach> Sachs { get; set; }
        public TheLoai() { this.Sachs = new HashSet<Sach>(); }
    }
}