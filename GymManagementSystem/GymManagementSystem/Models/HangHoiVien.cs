using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    public class HangHoiVien
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TenHang { get; set; } // Ví dụ: Bạc, Vàng, Kim Cương
        public decimal NguongChiTieu { get; set; }
        public string DacQuyen { get; set; } // Mô tả các đặc quyền
        public virtual ICollection<HangCoKhuyenMai> KhuyenMaiDacQuyen { get; set; }

    }
}