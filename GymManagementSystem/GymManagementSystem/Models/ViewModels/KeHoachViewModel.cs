// Trong file Models/ViewModels/KeHoachViewModel.cs
using System.Collections.Generic;
using System.Web.Mvc;

namespace GymManagementSystem.Models.ViewModels
{
    public class KeHoachViewModel
    {
        // Đối tượng KeHoach chính để binding dữ liệu
        public KeHoach KeHoach { get; set; }

        // Danh sách để đổ vào các DropDownList
        public IEnumerable<SelectListItem> DanhSachBaiTap { get; set; }
        public IEnumerable<SelectListItem> DanhSachKhuyenMai { get; set; }

        public KeHoachViewModel()
        {
            // Khởi tạo để tránh lỗi null, đặc biệt là danh sách chi tiết
            KeHoach = new KeHoach();
            KeHoach.ChiTietKeHoachs = new List<ChiTietKeHoach>();
        }
    }
}