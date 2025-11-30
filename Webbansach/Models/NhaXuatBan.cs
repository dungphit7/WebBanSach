using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webbansach.Models
{
    public class NhaXuatBan
    {
        [Key] public int MaNXB { get; set; }
        [Required, StringLength(100)] public string TenNXB { get; set; }
        public virtual ICollection<Sach> Sachs { get; set; }
        public NhaXuatBan() { this.Sachs = new HashSet<Sach>(); }
    }
}