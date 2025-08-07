using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{
    public enum TrangThai
    {
        ChoThanhToan,
        DaThanhToan,
        DaHuy
    }

    public class HoaDon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HoiVienId { get; set; }

        [Required]
        public int GoiTapId { get; set; }
        public int? KhuyenMaiId { get; set; }

        public DateTime NgayTao { get; set; }


        [Required]
        public decimal GiaGoc { get; set; }
        public decimal SoTienGiam { get; set; }

        [Required]
        public decimal ThanhTien { get; set; }

        [Required]
        public TrangThai TrangThai { get; set; }

        [ForeignKey("HoiVienId")]
        public virtual ApplicationUser HoiVien { get; set; }

        [ForeignKey("GoiTapId")]
        public virtual GoiTap GoiTap { get; set; }

        [ForeignKey("KhuyenMaiId")]
        public virtual KhuyenMai KhuyenMai { get; set; }

        public string MomoOrderId { get; set; }

        public string PayUrl { get; set; }
    }
}