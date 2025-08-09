// Trong file Models/ViewModels/HangHoiVienViewModel.cs
using System.Collections.Generic;
using System.Web.Mvc;

namespace GymManagementSystem.Models.ViewModels
{
    public class HangHoiVienViewModel
    {
        public HangHoiVien HangHoiVien { get; set; }

        // Dùng để nhận danh sách ID từ multi-select dropdown
        public List<int> SelectedKhuyenMaiIds { get; set; }

        // Dùng để đổ dữ liệu vào multi-select dropdown
        public IEnumerable<SelectListItem> DanhSachKhuyenMai { get; set; }

        public HangHoiVienViewModel()
        {
            HangHoiVien = new HangHoiVien();
            SelectedKhuyenMaiIds = new List<int>();
        }
    }
}