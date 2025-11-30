using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Webbansach.Models;

namespace Webbansach.Controllers
{
    public class DonHangsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ADMIN: Xem tất cả đơn hàng
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var donHangs = db.DonHangs.Include(d => d.ApplicationUser)
                                      .OrderByDescending(d => d.NgayDat)
                                      .ToList();
            return View(donHangs);
        }

        // ADMIN và User: xem chi tiết đơn hàng (và chỉ chủ đơn xem được đơn của mình)
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
                return HttpNotFound();

            var currentUserId = User.Identity.GetUserId();
            var userRole = (User.IsInRole("Admin") ? "Admin" : "User");

            // Admin xem hết, User chỉ xem đơn của mình
            if (userRole == "Admin" || donHang.UserId == currentUserId)
            {
                // Hiện luôn phần chi tiết
                var chiTietDonHangs = db.ChiTietDonHangs.Where(c => c.MaDonHang == donHang.MaDonHang).ToList();
                ViewBag.ChiTietDonHang = chiTietDonHangs;

                return View(donHang);
            }
            else
            {
                // Không hợp lệ → về trang lịch sử mua hàng của user
                return RedirectToAction("MyOrders");
            }
        }

        // USER: Xem lịch sử của tôi
        [Authorize]
        public ActionResult MyOrders()
        {
            var currentUserId = User.Identity.GetUserId();

            var donHangs = db.DonHangs
                            .Where(d => d.UserId == currentUserId)
                            .OrderByDescending(d => d.NgayDat)
                            .ToList();

            return View(donHangs);
        }

        // ADMIN: Sửa trạng thái
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id )
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
                return HttpNotFound();

            ViewBag.UserId = new SelectList(db.Users, "Id", "HoTen", donHang.UserId);
            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
      
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int MaDonHang, string status)
        {
            var donHangInDb = db.DonHangs.Find(MaDonHang);
            if (donHangInDb == null)
                return HttpNotFound();

            donHangInDb.TrangThai = status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // ADMIN: Xóa đơn
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
                return HttpNotFound();

            return View(donHang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
