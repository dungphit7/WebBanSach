using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webbansach.Models
{
    public class DatTruocViewModel
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }

        [Required]
        public string HoTen { get; set; }

        [Required]
        public string SDT { get; set; }

        [Required]
        public string DiaChi { get; set; }

        public string Email { get; set; }
        public string GhiChu { get; set; }
    }
}