using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Webbansach.Models;
namespace Webbansach.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int page = 1)
        {
            int pageSize = 12; // số sách mỗi trang
            int totalSachs = db.Sachs.Count();
            int totalPages = (int)Math.Ceiling((double)totalSachs / pageSize);

            // Bảo vệ page nhỏ hơn 1 hoặc lớn hơn tổng số trang
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var sachs = db.Sachs
                .OrderByDescending(s => s.MaSach)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(s => s.NhaXuatBan)
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // Logic lấy sách hôm nay (giống như đã bàn)

            var today = DateTime.Today;

            var tomorrow = today.AddDays(1);



            var sachMoiHomNay = db.Sachs

                .Include(s => s.NhaXuatBan) // Include nếu cần hiển thị tên NXB

                .Where(s => s.CreateAt >= today && s.CreateAt < tomorrow)

                .OrderByDescending(s => s.CreateAt) // Mới nhất lên đầu

                .ToList();



            // Nếu hôm nay chưa có sách nào, có thể lấy tạm 8 cuốn mới nhất bất kỳ (Optional)

            if (sachMoiHomNay.Count == 0)

            {

                ViewBag.Message = "Hôm nay chưa có sách mới, dưới đây là các sách nổi bật.";

                sachMoiHomNay = db.Sachs.OrderByDescending(s => s.MaSach).Take(8).ToList();

            }

            else

            {

                ViewBag.Message = "Sách vừa cập nhật hôm nay";

            }



            return View(sachMoiHomNay);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Chinhsachbaomat()
        {
            return View();
        }
        public ActionResult Chinhsachdoitra()
        {
            return View();
        }
        public ActionResult Chinhsachthanhtoan()
        {
            return View();
        }

    }
}