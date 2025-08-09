using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GymManagementSystem.Models.ViewModels
{
    public class XacNhanThanhToanViewModel
    {
        public HoaDon HoaDon { get; set; }

        [Display(Name = "Chọn khuyến mãi của bạn")]
        public int? KhuyenMaiCuaHoiVienId { get; set; }

        public IEnumerable<SelectListItem> DanhSachKhuyenMai { get; set; }

        public KhuyenMai KhuyenMaiDaApDung { get; set; }
    }
}