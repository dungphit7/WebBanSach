using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Webbansach.Models;

namespace Webbansach.Controllers
{
    public class SachesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Saches

        public ActionResult Index(int page = 1)
        {

            int pageSize = 12; // Số sách mỗi trang

            var sachsAll = db.Sachs
                .Include(s => s.NhaXuatBan)
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .OrderBy(s => s.MaSach);

            int totalProducts = sachsAll.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var sachs = sachsAll
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(sachs);
        }

        // GET: Saches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Sachs.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllSaches = db.Sachs.Where(x => x.MaSach != id).Take(5).ToList();
            return View(sach);
        }

        // GET: Saches/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai");
            return View();
        }

        // POST: Saches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "MaSach,TenSach,MoTa,GiaBan,SoLuongTon,MaTheLoai,MaTacGia,MaNXB")] Sach sach, HttpPostedFileBase Upload)
        {
            if (Upload != null && Upload.ContentLength > 0)
            {
                try
                {
                    sach.HinhAnh = SaveImageAndGetUrl(Upload); // Lưu ảnh và lấy đường dẫn
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("HinhAnh", ex.Message);
                }
            }

            if (ModelState.IsValid)
            {
                sach.CreateAt = DateTime.Now;
                db.Sachs.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia", sach.MaTacGia);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            return View(sach);
        }

        // GET: Saches/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Sachs.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia", sach.MaTacGia);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            return View(sach);
        }

        // POST: Saches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit([Bind(Include = "MaSach,TenSach,MoTa,HinhAnh,GiaBan,SoLuongTon,MaTheLoai,MaTacGia,MaNXB")] Sach sach, HttpPostedFileBase Upload)
        {
            if (ModelState.IsValid)
            {
                // Lấy sách cũ từ DB
                var sachCu = db.Sachs.Find(sach.MaSach);
                if (sachCu == null) return HttpNotFound();

                // Nếu người dùng upload ảnh mới, lưu lại ảnh mới và cập nhật đường dẫn
                if (Upload != null && Upload.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(Upload.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/Uploads/Sachs"), fileName);
                    Upload.SaveAs(path);
                    sachCu.HinhAnh = "~/Content/Uploads/Sachs/" + fileName;
                }
                // Nếu không chọn ảnh mới => vẫn giữ lại đường dẫn ảnh cũ, KHÔNG set lại bằng null

                // Cập nhật các thuộc tính khác
                sachCu.TenSach = sach.TenSach;
                sachCu.MoTa = sach.MoTa;
                sachCu.GiaBan = sach.GiaBan;
                sachCu.SoLuongTon = sach.SoLuongTon;
                sachCu.MaTheLoai = sach.MaTheLoai;
                sachCu.MaTacGia = sach.MaTacGia;
                sachCu.MaNXB = sach.MaNXB;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sach.MaNXB);
            ViewBag.MaTacGia = new SelectList(db.TacGias, "MaTacGia", "TenTacGia", sach.MaTacGia);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", sach.MaTheLoai);
            return View(sach);
        }


        // GET: Saches/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Sachs.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Saches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Sach sach = db.Sachs.Find(id);
            db.Sachs.Remove(sach);
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
        private void PopulateSelections(Sach sach = null)
        {
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.OrderBy(t => t.TenTheLoai),
                                               "MaTheLoai", "TenTheLoai", sach?.MaTheLoai);
            // Nếu có thêm nhiều lựa chọn khác cho sách, thêm ở đây
        }

        // Vẫn giữ nguyên kiểm tra extensions
        private static readonly string[] _allowedExtensions =
            { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        private string SaveImageAndGetUrl(System.Web.HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0) return null;

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
                throw new InvalidOperationException("Chỉ cho phép ảnh jpg, jpeg, png, gif, webp");

            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("File upload không phải ảnh.");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var virtualDir = "~/Uploads/Sachs/"; // CHUYỂN SANG Sách
            var physicalDir = Server.MapPath(virtualDir);
            Directory.CreateDirectory(physicalDir);

            var physicalPath = Path.Combine(physicalDir, fileName);
            file.SaveAs(physicalPath);

            return VirtualPathUtility.ToAbsolute($"{virtualDir}/{fileName}");
        }

        private void DeleteImageIfExists(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return;
            var physicalPath = Server.MapPath(imageUrl);
            if (System.IO.File.Exists(physicalPath))
            {
                try { System.IO.File.Delete(physicalPath); } catch { }
            }
        }
        public ActionResult Search(string query, int page = 1)
        {
            int pageSize = 12;
            var matched = db.Sachs
                .Include(s => s.NhaXuatBan)
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .Where(s => s.TenSach.Contains(query))
                .OrderBy(s => s.MaSach);

            int totalProducts = matched.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var sachs = matched
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = query;

            return View("Index", sachs); // Tận dụng view Index.cshtml để hiển thị kết quả
        }

        // GET: Saches/BooksToday
        public ActionResult BooksToday(int page = 1)
        {
            int pageSize = 12; // Giữ nguyên số lượng sách mỗi trang giống Index

            // Xác định khoảng thời gian của "Hôm nay"
            // Từ 00:00:00 hôm nay đến 00:00:00 ngày mai
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            // Query lấy sách
            var matched = db.Sachs
                .Include(s => s.NhaXuatBan)
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                // Lọc những sách có CreateAt >= đầu ngày VÀ < đầu ngày mai
                .Where(s => s.CreateAt >= today && s.CreateAt < tomorrow)
                // Sắp xếp sách mới nhất lên đầu
                .OrderByDescending(s => s.CreateAt);

            // Xử lý phân trang (Giống hệt Index)
            int totalProducts = matched.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var sachs = matched
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // Thêm một ViewBag để View biết đây là danh sách "Sách mới hôm nay" (tùy chọn hiển thị tiêu đề)
            ViewBag.TitleHeader = "Sách vừa cập nhật hôm nay";

            // Tận dụng lại View Index.cshtml để hiển thị danh sách
            return View("Index", sachs);
        }


    }
}