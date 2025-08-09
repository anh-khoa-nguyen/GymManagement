using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    public enum TinhTrangThietBi
    {
        [Display(Name = "Hoạt động tốt")]
        HoatDongTot,
        [Display(Name = "Cần bảo trì")]
        CanBaoTri,
        [Display(Name = "Đang sửa chữa")]
        DangSuaChua,
        [Display(Name = "Đã hỏng")]
        Hong
    }

    public class ThietBi
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100), Display(Name = "Tên Thiết Bị")]
        public string TenThietBi { get; set; }

        [DataType(DataType.MultilineText), Display(Name = "Mô Tả")]
        public string MoTa { get; set; }

        [DataType(DataType.Date), Display(Name = "Ngày Mua")]
        public DateTime? NgayMua { get; set; }

        [Required, Display(Name = "Tình Trạng")]
        public TinhTrangThietBi TinhTrang { get; set; }
    }
}