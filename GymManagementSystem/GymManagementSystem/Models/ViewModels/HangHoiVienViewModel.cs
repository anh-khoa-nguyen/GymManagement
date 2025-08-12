// Trong file Models/ViewModels/HangHoiVienViewModel.cs
using System.Collections.Generic;
using System.Web.Mvc;

namespace GymManagementSystem.Models.ViewModels
{
    public class HangHoiVienViewModel
    {
        public HangHoiVien HangHoiVien { get; set; }

        public List<int> SelectedKhuyenMaiIds { get; set; }
        public IEnumerable<SelectListItem> DanhSachKhuyenMai { get; set; }

        public List<int> SelectedDacQuyenIds { get; set; }
        public IEnumerable<SelectListItem> DanhSachDacQuyen { get; set; }

        public HangHoiVienViewModel()
        {
            HangHoiVien = new HangHoiVien();
            SelectedKhuyenMaiIds = new List<int>();
            SelectedDacQuyenIds = new List<int>(); // Khởi tạo
        }
    }
}