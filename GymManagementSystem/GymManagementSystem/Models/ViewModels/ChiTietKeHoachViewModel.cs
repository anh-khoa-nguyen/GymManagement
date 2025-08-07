// Trong file Models/ViewModels/ChiTietKeHoachViewModel.cs
using System.Collections.Generic;

namespace GymManagementSystem.Models.ViewModels
{
    public class NgayTapLuyenItem
    {
        public int NgayTrongKeHoach { get; set; }
        public ChiTietKeHoach ChiTietBaiTap { get; set; }
        public bool DaHoanThanh { get; set; }
        public bool CoTheTap { get; set; }
    }

    // ViewModel chính cho cả trang
    public class ChiTietKeHoachViewModel
    {
        public DangKyKeHoach DangKyKeHoach { get; set; }
        public List<NgayTapLuyenItem> DanhSachNgayTap { get; set; }
        public int PhanTramHoanThanh { get; set; }
    }
}