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
    public class ChiTietDonHangsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChiTietDonHangs
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var chiTietDonHangs = db.ChiTietDonHangs.Include(c => c.DonHang).Include(c => c.Sach);
            return View(chiTietDonHangs.ToList());
        }

        // GET: ChiTietDonHangs/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs
                .Include(x => x.DonHang)
                .Include(x => x.Sach)
                .FirstOrDefault(x => x.MaCTDH == id);

            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }

            // Bảo mật: Khách chỉ xem Order của chính mình!
            // (Nếu action này được lộ ra ngoài)
            var currentUserId = User.Identity.IsAuthenticated
                ? User.Identity.GetUserId()
                : null;

            if (!User.IsInRole("Admin") && chiTietDonHang.DonHang.UserId != currentUserId)
            {
                return RedirectToAction("MyOrders", "DonHangs");
            }

            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.MaDonHang = new SelectList(db.DonHangs, "MaDonHang", "TrangThai");
            ViewBag.MaSach = new SelectList(db.Sachs, "MaSach", "TenSach");
            return View();
        }

        // POST: ChiTietDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "MaCTDH,SoLuong,DonGia,MaDonHang,MaSach")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietDonHangs.Add(chiTietDonHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDonHang = new SelectList(db.DonHangs, "MaDonHang", "TrangThai", chiTietDonHang.MaDonHang);
            ViewBag.MaSach = new SelectList(db.Sachs, "MaSach", "TenSach", chiTietDonHang.MaSach);
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.Find(id);
            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDonHang = new SelectList(db.DonHangs, "MaDonHang", "TrangThai", chiTietDonHang.MaDonHang);
            ViewBag.MaSach = new SelectList(db.Sachs, "MaSach", "TenSach", chiTietDonHang.MaSach);
            return View(chiTietDonHang);
        }

        // POST: ChiTietDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "MaCTDH,SoLuong,DonGia,MaDonHang,MaSach")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietDonHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDonHang = new SelectList(db.DonHangs, "MaDonHang", "TrangThai", chiTietDonHang.MaDonHang);
            ViewBag.MaSach = new SelectList(db.Sachs, "MaSach", "TenSach", chiTietDonHang.MaSach);
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.Find(id);
            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDonHang);
        }

        // POST: ChiTietDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.Find(id);
            db.ChiTietDonHangs.Remove(chiTietDonHang);
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
