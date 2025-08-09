// Trong file Models/ViewModels/ChiTietKeHoachViewModel.cs
using System.Collections.Generic;

namespace GymManagementSystem.Models.ViewModels
{
    public class NgayTapLuyenItem
    {
        public int NgayTrongKeHoach { get; set; }
        public ChiTietKeHoach ChiTietBaiTap { get; set; } //1 bài tập
        public bool DaHoanThanh { get; set; } //Dựa vào model: TienDoBaiTap
        public bool CoTheTap { get; set; } //Dựa vào model: TienDoBaiTap
    }

    // ViewModel chính cho cả trang
    public class ChiTietKeHoachViewModel
    {
        public DangKyKeHoach DangKyKeHoach { get; set; }
        public List<NgayTapLuyenItem> DanhSachNgayTap { get; set; }
        public int PhanTramHoanThanh { get; set; } //% Toàn bộ kế hoạch
    }
}