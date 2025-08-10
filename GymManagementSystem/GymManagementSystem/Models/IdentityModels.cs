using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GymManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } 

        [Required]
        public string VaiTro { get; set; } //  "QuanLy", "PT", "HoiVien"

        // FK
        public int? HoiVienId { get; set; }

        public int? HuanLuyenVienId { get; set; }

        //public int? HangHoiVienId { get; set; }

        //[ForeignKey("HangHoiVienId")]
        //public virtual HangHoiVien HangHoiVien { get; set; }

        public string NguoiGioiThieuId { get; set; }

        [Display(Name = "Ảnh Đại Diện")]
        [DataType(DataType.Url)]
        public string AvatarUrl { get; set; }

        public virtual ICollection<ThongBao> ThongBaos { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
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

        //Khoa
        public DbSet<KeHoach> KeHoachs { get; set; }
        public DbSet<ChiTietKeHoach> ChiTietKeHoachs { get; set; }
        public DbSet<DangKyKeHoach> DangKyKeHoachs { get; set; }

        public DbSet<BaiTap> BaiTaps { get; set; }
        public DbSet<BuocThucHien> BuocThucHiens { get; set; }
        public DbSet<TienDoBaiTap> TienDoBaiTaps { get; set; }
        public DbSet<HangHoiVien> HangHoiViens { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<HangCoKhuyenMai> HangHoiVien_KhuyenMais { get; set; }
        public virtual DbSet<KhuyenMaiCuaGoi> KhuyenMaiCuaGois { get; set; }

        public DbSet<HangCoDacQuyen> HangHoiVien_DacQuyens { get; set; }

        public DbSet<KhuyenMaiCuaHoiVien> KhuyenMaiCuaHoiViens { get; set; }
        public DbSet<ThietBi> ThietBis { get; set; }
        public DbSet<LichSuThietBi> LichSuThietBis { get; set; }
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<LichSuCheckin> LichSuCheckins { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<GymManagementSystem.Models.DacQuyen> DacQuyens { get; set; }
    }
}