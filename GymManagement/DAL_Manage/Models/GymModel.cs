using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL_Manage.Models
{
    public partial class GymModel : DbContext
    {
        public GymModel()
            : base("name=GymModel")
        {
        }

        public virtual DbSet<DangKy> DangKy { get; set; }
        public virtual DbSet<GoiTap> GoiTap { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public virtual DbSet<HoiVien> HoiVien { get; set; }
        public virtual DbSet<LichTap> LichTap { get; set; }
        public virtual DbSet<LoaiGoiTap> LoaiGoiTap { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<PhieuKiemTra> PhieuKiemTra { get; set; }
        public virtual DbSet<Phong> Phong { get; set; }
        public virtual DbSet<TienDoTapLuyen> TienDoTapLuyen { get; set; }
        public virtual DbSet<ThietBi> ThietBi { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoiTap>()
                .Property(e => e.Gia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<GoiTap>()
                .HasMany(e => e.DangKy)
                .WithRequired(e => e.GoiTap)
                .HasForeignKey(e => e.ID_Goi_Tap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GoiTap>()
                .HasMany(e => e.HoaDon)
                .WithOptional(e => e.GoiTap)
                .HasForeignKey(e => e.ID_GoiTap);

            modelBuilder.Entity<HoiVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<HoiVien>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<HoiVien>()
                .HasMany(e => e.DangKy)
                .WithRequired(e => e.HoiVien)
                .HasForeignKey(e => e.ID_HoiVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HoiVien>()
                .HasMany(e => e.HoaDon)
                .WithOptional(e => e.HoiVien)
                .HasForeignKey(e => e.ID_HoiVien);

            modelBuilder.Entity<HoiVien>()
                .HasMany(e => e.LichTap)
                .WithOptional(e => e.HoiVien)
                .HasForeignKey(e => e.ID_HoiVien);

            modelBuilder.Entity<HoiVien>()
                .HasMany(e => e.TienDoTapLuyen)
                .WithOptional(e => e.HoiVien)
                .HasForeignKey(e => e.ID_HoiVien);

            modelBuilder.Entity<LoaiGoiTap>()
                .HasMany(e => e.GoiTap)
                .WithOptional(e => e.LoaiGoiTap)
                .HasForeignKey(e => e.ID_Loai_Goi_Tap);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .HasMany(e => e.HoaDon)
                .WithOptional(e => e.NhanVien)
                .HasForeignKey(e => e.ID_Nhan_Vien);

            modelBuilder.Entity<NhanVien>()
                .HasMany(e => e.LichTap)
                .WithOptional(e => e.NhanVien)
                .HasForeignKey(e => e.ID_Nhan_Vien);

            modelBuilder.Entity<NhanVien>()
                .HasMany(e => e.PhieuKiemTra)
                .WithOptional(e => e.NhanVien)
                .HasForeignKey(e => e.ID_Nhan_Vien);

            modelBuilder.Entity<NhanVien>()
                .HasMany(e => e.TienDoTapLuyen)
                .WithOptional(e => e.NhanVien)
                .HasForeignKey(e => e.ID_Nhan_Vien);

            modelBuilder.Entity<Phong>()
                .HasMany(e => e.NhanVien)
                .WithOptional(e => e.Phong)
                .HasForeignKey(e => e.ID_Phong);

            modelBuilder.Entity<Phong>()
                .HasMany(e => e.ThietBi)
                .WithOptional(e => e.Phong)
                .HasForeignKey(e => e.ID_Phong);

            modelBuilder.Entity<TienDoTapLuyen>()
                .Property(e => e.Can_Nang)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TienDoTapLuyen>()
                .Property(e => e.Ti_Le_Mo)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ThietBi>()
                .HasMany(e => e.PhieuKiemTra)
                .WithOptional(e => e.ThietBi)
                .HasForeignKey(e => e.ID_Thiet_Bi);
        }
    }
}
