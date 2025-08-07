using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Models
{
    public class TienDoBaiTap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DangKyKeHoachId { get; set; }

        [Required]
        public int BaiTapId { get; set; } // Foreign key tới BaiTap.Id

        public DateTime NgayHoanThanh { get; set; }

        [ForeignKey("DangKyKeHoachId")]
        public virtual DangKyKeHoach DangKyKeHoach { get; set; }

        [ForeignKey("BaiTapId")]
        public virtual BaiTap BaiTap { get; set; }
    }
}