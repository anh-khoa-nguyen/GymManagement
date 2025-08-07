using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    public class ChiSoSucKhoe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HoiVienId { get; set; }

        [Required]
        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.Date)]
        public DateTime NgayCapNhat { get; set; }

        [Required]
        [Display(Name = "Cân nặng (kg)")]
        public double CanNang { get; set; }

        [Display(Name = "Tỷ lệ mỡ (%)")]
        public double? TyLeMo { get; set; }

        [ForeignKey("HoiVienId")]
        public virtual HoiVien HoiVien { get; set; }
    }
}