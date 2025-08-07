using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GymManagementSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } 

        [Required]
        public string VaiTro { get; set; } //  "QuanLy", "PT", "HoiVien"

        // Các khóa ngoại để liên kết tới các bảng hồ sơ chi tiết
        public int? HoiVienId { get; set; }
        public int? HuanLuyenVienId { get; set; }
        public virtual ICollection<ThongBao> ThongBaos { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<GoiTap> GoiTaps { get; set; }
        public DbSet<HoiVien> HoiViens { get; set; }
        public DbSet<HuanLuyenVien> HuanLuyenViens { get; set; }
        public DbSet<DangKyGoiTap> DangKyGoiTaps { get; set; }
        public DbSet<LichTap> LichTaps { get; set; }
        public DbSet<ChiSoSucKhoe> ChiSoSucKhoes { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}