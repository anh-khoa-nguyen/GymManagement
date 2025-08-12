using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    public class KhuyenMai
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhuyenMai { get; set; }

        public string MoTa { get; set; }

        [Required]
        [StringLength(50)]
        public string MaKhuyenMai { get; set; }

        public double PhanTramGiamGia { get; set; }

        [Display(Name = "Số Tiền Giảm Tối Đa")]
        public decimal SoTienGiamToiDa { get; set; }

        [Display(Name = "Thời hạn sử dụng (ngày)")]
        public int SoNgayHieuLuc { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<HangCoKhuyenMai> ApDungChoHangHoiVien { get; set; }
        public virtual ICollection<KhuyenMaiCuaGoi> ApDungChoGoiTap { get; set; }

    }
}