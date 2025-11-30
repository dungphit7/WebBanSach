using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Webbansach.Models; 
namespace Webbansach.Models
{
    public class DonHang
    {
        [Key] public int MaDonHang { get; set; }
        public DateTime? NgayDat { get; set; }
        public decimal? TongTien { get; set; }
        [StringLength(50)] public string TrangThai { get; set; }
        [StringLength(255)] public string DiaChiNhan { get; set; }
        [StringLength(15)] public string SdtNhan { get; set; }

        public string StudentId { get; set; }    // Mã sinh viên
        public string University { get; set; }
        [StringLength(100)]
        public string TenNguoiNhan { get; set; }

        [StringLength(100)]
        public string Quocgia { get; set; }
        [StringLength(100)]
        public string Tpho { get; set; }

        [StringLength(100)]  public string Phuong { get; set; }

        [StringLength(100)] public string ShippingMethod { get; set; }
        [StringLength(100)] public string PaymentMethod { get; set; }


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        // ----------------------------

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        // Constructor mặc định là bắt buộc cho EF
        public DonHang()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }
    }
}