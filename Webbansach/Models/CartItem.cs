using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webbansach.Models
{
    public class CartItem
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public decimal GiaBan { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; } // Số lượng đặt mua

        public decimal ThanhTien => GiaBan * SoLuong;
    }

}