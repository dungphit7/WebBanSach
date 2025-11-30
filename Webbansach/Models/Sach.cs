using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Webbansach.Models
{
    public class Sach
    {
        [Key] public int MaSach { get; set; }

        [Required, StringLength(255)] public string TenSach { get; set; }
        [Column(TypeName = "ntext")] public string MoTa { get; set; }
        [StringLength(255)] public string HinhAnh { get; set; }
        [Required] public decimal GiaBan { get; set; }
        [Required] public int SoLuongTon { get; set; }
        public DateTime? CreateAt { get; set; }

        // Khóa ngoại
        public int? MaTheLoai { get; set; }
        public int? MaTacGia { get; set; }
        public int? MaNXB { get; set; }

        // Navigation properties
        [ForeignKey("MaTheLoai")] public virtual TheLoai TheLoai { get; set; }
        [ForeignKey("MaTacGia")] public virtual TacGia TacGia { get; set; }
        [ForeignKey("MaNXB")] public virtual NhaXuatBan NhaXuatBan { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public Sach() { this.ChiTietDonHangs = new HashSet<ChiTietDonHang>(); }
    }
}