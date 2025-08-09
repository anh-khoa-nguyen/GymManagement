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
        public int KeHoachId { get; set; }

        [Required]
        public int BaiTapId { get; set; }

        public int NgayTrongKeHoach { get; set; } // Ví dụ: 1, 2, 3, ..., 30

        public int SoLanMucTieu { get; set; }

        [ForeignKey("KeHoachId")]
        public virtual KeHoach KeHoach { get; set; }

        [ForeignKey("BaiTapId")]
        public virtual BaiTap BaiTap { get; set; }
    }
}