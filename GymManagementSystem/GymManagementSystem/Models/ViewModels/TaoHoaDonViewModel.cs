using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GymManagementSystem.Models.ViewModels
{
    public class TaoHoaDonViewModel
    {
        // Các trường người dùng sẽ nhập hoặc chọn
        [Required(ErrorMessage = "Vui lòng chọn hội viên.")]
        public string HoiVienId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn gói tập.")]
        public int GoiTapId { get; set; }

        public int? KhuyenMaiId { get; set; }

        // Các danh sách để đổ vào DropDownList
        public IEnumerable<SelectListItem> DanhSachHoiVien { get; set; }
        public IEnumerable<SelectListItem> DanhSachGoiTap { get; set; }
        public IEnumerable<SelectListItem> DanhSachKhuyenMai { get; set; }
    }
}