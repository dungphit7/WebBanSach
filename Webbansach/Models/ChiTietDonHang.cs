using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Webbansach.Models
{
    public class ChiTietDonHang
    {
        [Key] public int MaCTDH { get; set; }
        [Required] public int SoLuong { get; set; }
        [Required] public decimal DonGia { get; set; }

        // Khóa ngoại
        public int MaDonHang { get; set; }
        public int MaSach { get; set; }

        // Navigation properties
        [ForeignKey("MaDonHang")] public virtual DonHang DonHang { get; set; }
        [ForeignKey("MaSach")] public virtual Sach Sach { get; set; }
    }
}