using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Webbansach.Models;

namespace Webbansach.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // Lấy giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // Thêm sách vào giỏ
        public ActionResult AddToCart(int id)
        {
            var db = new ApplicationDbContext();
            var sach = db.Sachs.Find(id);
            if (sach == null) return HttpNotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSach == id);
            if (item != null)
                item.SoLuong++;
            else
                cart.Add(new CartItem
                {
                    MaSach = sach.MaSach,
                    TenSach = sach.TenSach,
                    GiaBan = sach.GiaBan,
                    HinhAnh = sach.HinhAnh,
                    SoLuong = 1
                });
            return RedirectToAction("Index");
        }

        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCart();
            ViewBag.Total = cart.Sum(i => i.GiaBan * i.SoLuong);
            return View(cart);
        }

        // Xóa sách khỏi giỏ
        public ActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSach == id);
            if (item != null) cart.Remove(item);
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng sách trong giỏ
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSach == id);
            if (item != null && quantity > 0)
                item.SoLuong = quantity;
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index", "Home");
            ViewBag.Total = cart.Sum(i => i.GiaBan * i.SoLuong);
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]

        public ActionResult Checkout(string TenNguoiNhan, string DiaChiNhan, string SdtNhan,string Quocgia,string Tpho,string Phuong, string ShippingMethod, string PaymentMethod, string studentId, string university)
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index", "Home");
            decimal originalTotal = cart.Sum(i => i.GiaBan * i.SoLuong);

            var order = new DonHang
            {
                TenNguoiNhan = TenNguoiNhan,
                DiaChiNhan = DiaChiNhan,
                SdtNhan = SdtNhan,
                Quocgia= Quocgia,
                Tpho= Tpho,
                Phuong= Phuong,
                ShippingMethod = ShippingMethod,
                PaymentMethod = PaymentMethod,

                NgayDat = DateTime.Now,
                UserId = User.Identity.GetUserId(),
                TongTien = cart.Sum(i => i.GiaBan * i.SoLuong)
            };
            // Giảm giá nếu có studentId
            if (!string.IsNullOrEmpty(studentId))
            {
                order.TongTien = originalTotal * 0.8m;
                order.StudentId = studentId;
                order.University = university;
            }
            else
            {
                order.TongTien = originalTotal;
            }
            db.DonHangs.Add(order);
            db.SaveChanges();

            foreach (var item in cart)
            {
                var detail = new ChiTietDonHang
                {
                    MaDonHang = order.MaDonHang, // Sử dụng đúng property của DonHang
                    MaSach = item.MaSach,
                    SoLuong = item.SoLuong,
                    DonGia = item.GiaBan
                };
                db.ChiTietDonHangs.Add(detail);
                // TRỪ TỒN KHO SÁCH
                var sachInDb = db.Sachs.Find(item.MaSach);
                if (sachInDb != null)
                {
                    sachInDb.SoLuongTon = sachInDb.SoLuongTon - item.SoLuong;
                }
            }
            db.SaveChanges();

            Session["Cart"] = null;
            TempData["Message"] = "Đặt hàng thành công!";
            return RedirectToAction("Success");
        }
        [AllowAnonymous]
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult PreOrder(int id)
        {
            // Lấy danh sách trong giỏ hàng (hoặc tạo mới nếu chưa có)
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            // Kiểm tra nếu sách đã có trong giỏ thì không thêm nữa
            var item = cart.FirstOrDefault(x => x.MaSach == id);
            if (item == null)
            {
                var sach = db.Sachs.Find(id);
                if (sach != null)
                {
                    cart.Add(new CartItem
                    {
                        MaSach = sach.MaSach,
                        TenSach = sach.TenSach,
                        GiaBan = sach.GiaBan,
                        HinhAnh = sach.HinhAnh,
                        SoLuong = 1
                    });
                }
            }
            Session["Cart"] = cart;

            // Chuyển thẳng sang trang Checkout
            return RedirectToAction("Checkout");
        }



    }
}