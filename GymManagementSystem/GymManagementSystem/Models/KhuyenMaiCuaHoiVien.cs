// Trong file Models/KhuyenMaiCuaHoiVien.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{
    public enum TrangThaiKhuyenMaiHV
    {
        ChuaSuDung,
        DaSuDung,
        DaHetHan
    }

    public class KhuyenMaiCuaHoiVien
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HoiVienId { get; set; }

        [Required]
        public int KhuyenMaiId { get; set; }

        public DateTime NgayNhan { get; set; }

        public DateTime NgayHetHan { get; set; }

        [Required]
        public TrangThaiKhuyenMaiHV TrangThai { get; set; }

        [ForeignKey("HoiVienId")]
        public virtual HoiVien HoiVien { get; set; }

        [ForeignKey("KhuyenMaiId")]
        public virtual KhuyenMai KhuyenMai { get; set; }
    }
}