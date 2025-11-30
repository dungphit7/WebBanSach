using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Webbansach.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }

        // === 2. MỐI QUAN HỆ VỚI ĐƠN HÀNG (BẮT BUỘC) ===
        public virtual ICollection<DonHang> DonHangs { get; set; }

        // === 3. CONSTRUCTOR (NÊN CÓ) ===
        public ApplicationUser()
        {
            this.DonHangs = new HashSet<DonHang>();
        }

        // === 4. PHƯƠNG THỨC IDENTITY (CÓ SẴN) ===
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null);        
        }
        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public DbSet<Sach> Sachs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
    

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}