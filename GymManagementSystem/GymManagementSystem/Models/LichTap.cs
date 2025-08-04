using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    public enum TrangThaiLichTap
    {
        [Display(Name = "Chờ duyệt")]
        ChoDuyet,
        [Display(Name = "Đã duyệt")]
        DaDuyet,
        [Display(Name = "Đã hủy")]
        DaHuy,
        [Display(Name = "Đã hoàn thành")]
        DaHoanThanh
    }

    public class LichTap
    {
        [Key]
        public int Id { get; set; }

        // Khóa ngoại đến Hội viên đặt lịch
        [Required]
        public int HoiVienId { get; set; }

        // Khóa ngoại đến PT được đặt (có thể null nếu là lịch tự tập)
        public int? HuanLuyenVienId { get; set; }

        [Required]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime ThoiGianBatDau { get; set; }

        [Required]
        [Display(Name = "Thời gian kết thúc")]
        public DateTime ThoiGianKetThuc { get; set; }

        [Required]
        public TrangThaiLichTap TrangThai { get; set; }

        [Display(Name = "Ghi chú của hội viên")]
        public string GhiChuHoiVien { get; set; }

        [Display(Name = "Phản hồi của PT")]
        public string GhiChuPT { get; set; }

        // Navigation properties
        [ForeignKey("HoiVienId")]
        public virtual HoiVien HoiVien { get; set; }

        [ForeignKey("HuanLuyenVienId")]
        public virtual HuanLuyenVien HuanLuyenVien { get; set; }
    }
}