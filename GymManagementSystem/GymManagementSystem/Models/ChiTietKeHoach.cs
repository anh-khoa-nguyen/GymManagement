using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{ 
    public class ChiTietKeHoach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KeHoachId { get; set; } // FK tới KeHoach

        [Required]
        public int BaiTapId { get; set; } // FK tới BaiTap

        // Quan trọng: Xác định bài tập này dành cho ngày thứ mấy trong kế hoạch
        public int NgayTrongKeHoach { get; set; } // Ví dụ: 1, 2, 3, ..., 30
        public int SoLanMucTieu { get; set; }

        // Navigation properties
        [ForeignKey("KeHoachId")]
        public virtual KeHoach KeHoach { get; set; }

        [ForeignKey("BaiTapId")]
        public virtual BaiTap BaiTap { get; set; }
    }
}