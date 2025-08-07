using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Models
{
    public class BuocThucHien
    {
        [Key]
        public int Id { get; set; }

        public int BaiTapId { get; set; }

        [Required]
        [DisplayName("Thứ Tự Bước")]
        public int ThuTuBuoc { get; set; } // Ví dụ: 1, 2, 3...

        [Required(ErrorMessage = "Nội dung bước thực hiện không được để trống")]
        [DisplayName("Nội Dung Hướng Dẫn")]
        [DataType(DataType.MultilineText)]
        public string NoiDung { get; set; }

        [ForeignKey("BaiTapId")]
        public virtual BaiTap BaiTap { get; set; }
    }
}