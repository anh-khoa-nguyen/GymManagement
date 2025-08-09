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

        [ForeignKey("HoiVienId")]
        public virtual HoiVien HoiVien { get; set; }

        [ForeignKey("GoiTapId")]
        public virtual GoiTap GoiTap { get; set; }
    }
}