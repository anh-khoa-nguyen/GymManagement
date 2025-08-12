using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;


namespace GymManagementSystem.Models.ViewModels
{
    public class DatLichViewModel
    {
        [Required]
        [Display(Name = "Huấn luyện viên")]
        public int HuanLuyenVienId { get; set; }

        [Required]
        [Display(Name = "Ngày hẹn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NgayDatLich { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn một khung giờ.")]
        [Display(Name = "Giờ bắt đầu")]
        public string GioBatDau { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        // Dùng để đổ dữ liệu vào dropdown
        public IEnumerable<SelectListItem> DanhSachPT { get; set; }
    }
}