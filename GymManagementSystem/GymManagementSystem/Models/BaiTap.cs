using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Models
{
    public class BaiTap
    {
        public BaiTap()
        {
            // Khởi tạo để tránh lỗi null khi thêm các bước thực hiện mới
            this.CacBuocThucHien = new HashSet<BuocThucHien>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên bài tập không được để trống")]
        [DisplayName("Tên Bài Tập")]
        [StringLength(150)]
        public string TenBaiTap { get; set; }

        [DisplayName("Mô Tả Ngắn")]
        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Nhóm cơ chính không được để trống")]
        [DisplayName("Nhóm Cơ Chính")]
        [StringLength(100)]
        public string NhomCoChinh { get; set; } // Ví dụ: "Ngực", "Lưng", "Chân"

        [DisplayName("Nhóm Cơ Phụ")]
        [StringLength(100)]
        public string NhomCoPhu { get; set; } // Ví dụ: "Tay sau", "Vai"

        [DisplayName("Dụng Cụ Cần Thiết")]
        [StringLength(200)]
        public string DungCu { get; set; } // Ví dụ: "Tạ đơn", "Ghế phẳng", "Không cần dụng cụ"

        [DisplayName("Mức Độ")]
        public string MucDo { get; set; } // Ví dụ: "Cơ bản", "Trung bình", "Nâng cao"

        [DisplayName("Link Ảnh Minh Họa")]
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; }

        // Rất quan trọng cho tính năng tracking camera sau này
        [DisplayName("Link Video Hướng Dẫn")]
        [DataType(DataType.Url)]
        public string VideoUrl { get; set; }

        [DisplayName("Logic Đếm Rep (JS Function Name)")]
        [StringLength(100)]
        public string RepCountingLogic { get; set; }

        // Navigation property: Một bài tập sẽ có nhiều bước thực hiện
        public virtual ICollection<BuocThucHien> CacBuocThucHien { get; set; }
    }
}