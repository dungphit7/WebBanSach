using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webbansach.Models
{
    public class TacGia
    {
        [Key] public int MaTacGia { get; set; }
        [Required, StringLength(100)] public string TenTacGia { get; set; }
        public virtual ICollection<Sach> Sachs { get; set; }
        public TacGia() { this.Sachs = new HashSet<Sach>(); }
    }
}