using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    public class DacQuyen
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100), Display(Name = "Tên Đặc Quyền")]
        public string TenDacQuyen { get; set; } // Ví dụ: "Giữ xe miễn phí", "Tặng 1 buổi PT"

        [DataType(DataType.MultilineText), Display(Name = "Mô Tả")]
        public string MoTa { get; set; }

        [StringLength(50), Display(Name = "Icon (Font Awesome)")]
        public string IconClass { get; set; } // Ví dụ: "fas fa-parking"

        // Navigation property
        public virtual ICollection<HangCoDacQuyen> HangCoDacQuyen { get; set; }
    }
}