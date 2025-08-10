using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GymManagementSystem.Models
{
    public class GoiTap
    {
        [Key] 
        public int Id { get; set; }

        [Display(Name = "Tên Gói Tập")]
        [Required(ErrorMessage = "Tên gói tập là bắt buộc.")]
        [StringLength(100)]
        public string TenGoi { get; set; }

        [Display(Name = "Giá Tiền")]
        [Required(ErrorMessage = "Giá tiền là bắt buộc.")]
        public decimal GiaTien { get; set; }

        [Display(Name = "Mô Tả Quyền Lợi")]
        [StringLength(500)]
        public string MoTaQuyenLoi { get; set; }

        [Display(Name = "Số Buổi Tập với PT")]
        public int SoBuoiTapVoiPT { get; set; }

        [Display(Name = "Ảnh Minh Họa")]
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; }

        public virtual ICollection<DangKyGoiTap> DangKyGoiTaps { get; set; }
        public virtual ICollection<KhuyenMaiCuaGoi> KhuyenMaisApDung { get; set; }



    }
}