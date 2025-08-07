using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    public class KhuyenMai
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhuyenMai { get; set; }

        public string MoTa { get; set; }

        // Ví dụ: GIAMGIA10, TANG1THANG
        [Required]
        [StringLength(50)]
        public string MaKhuyenMai { get; set; }

        public double PhanTramGiamGia { get; set; }
        public decimal SoTienGiamGia { get; set; }

        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public bool IsActive { get; set; }
        public virtual ICollection<HangCoKhuyenMai> ApDungChoHangHoiVien { get; set; }

    }
}