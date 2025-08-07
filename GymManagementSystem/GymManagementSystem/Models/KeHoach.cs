using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    public class KeHoach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string TenKeHoach { get; set; } // Ví dụ: "30 Ngày Khởi Động Cùng Gymer"

        public string MoTa { get; set; }

        public int ThoiGianThucHien { get; set; } // Số ngày để hoàn thành, ví dụ: 30

        public int? KhuyenMaiId { get; set; }
        [ForeignKey("KhuyenMaiId")]
        public virtual KhuyenMai KhuyenMai { get; set; }

        public bool IsActive { get; set; } // Kế hoạch này có đang được áp dụng không?

        // Navigation property: Một kế hoạch sẽ có nhiều chi tiết (bài tập)
        public virtual ICollection<ChiTietKeHoach> ChiTietKeHoachs { get; set; }

    }
}