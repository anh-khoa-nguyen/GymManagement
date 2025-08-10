// Trong file Models/LichSuThietBi.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{
    public enum LoaiHanhDong
    {
        [Display(Name = "Tạo mới")]
        TaoMoi,
        [Display(Name = "Cập nhật")]
        CapNhat,
        [Display(Name = "Xóa")]
        Xoa
    }

    public class LichSuThietBi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ThietBiId { get; set; } // Liên kết với thiết bị bị tác động

        [Required]
        public string NguoiThucHienId { get; set; } // Ai đã thực hiện thay đổi (Admin)

        [Required]
        public LoaiHanhDong HanhDong { get; set; } // Hành động là gì (Thêm/Sửa/Xóa)

        [Required]
        public DateTime NgayThucHien { get; set; } // Thời điểm thay đổi

        public string TrangThaiTruoc { get; set; }
        public string TrangThaiSau { get; set; }


        // Navigation properties
        [ForeignKey("NguoiThucHienId")]
        public virtual ApplicationUser NguoiThucHien { get; set; }
    }
}