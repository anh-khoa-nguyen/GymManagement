using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    public enum TrangThaiDangKy
    {
        HoatDong,
        DaHetHan,
        DaHuy
    }

    public class DangKyGoiTap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HoiVienId { get; set; }

        [Required]
        public int GoiTapId { get; set; }

        [Required]
        public DateTime NgayDangKy { get; set; }

        public DateTime NgayHetHan { get; set; }

        [Required]
        public TrangThaiDangKy TrangThai { get; set; }

        [Display(Name = "Tổng số buổi PT")]
        public int SoBuoiTapVoiPT { get; set; } // Sẽ được copy từ Gói Tập gốc khi mua

        [Display(Name = "Số buổi PT đã sử dụng")]
        public int SoBuoiPTDaSuDung { get; set; } = 0; // Mặc định là 0 khi mới đăng ký

        [ForeignKey("HoiVienId")]
        public virtual HoiVien HoiVien { get; set; }

        [ForeignKey("GoiTapId")]
        public virtual GoiTap GoiTap { get; set; }
    }
}