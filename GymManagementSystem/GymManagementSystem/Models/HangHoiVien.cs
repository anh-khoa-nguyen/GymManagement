using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    public class HangHoiVien
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50), Display(Name = "Tên Đặc Quyền")]
        public string TenHang { get; set; } // Ví dụ: Bạc, Vàng, Kim Cương
        [Display(Name = "Ngưỡng Chi Tiêu")]
        public decimal NguongChiTieu { get; set; }
        public virtual ICollection<HangCoKhuyenMai> KhuyenMaiDacQuyen { get; set; }
        public virtual ICollection<HangCoDacQuyen> HangCoDacQuyen { get; set; }

    }
}