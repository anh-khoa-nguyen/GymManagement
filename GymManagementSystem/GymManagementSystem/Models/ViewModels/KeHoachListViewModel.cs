using System.Collections.Generic;

namespace GymManagementSystem.Models.ViewModels
{
    public class KeHoachListViewModel
    {
        public IEnumerable<KeHoach> DaDangKy { get; set; } //Model: Đăng kí kế hoạch

        public IEnumerable<KeHoach> ChuaDangKy { get; set; } //Model: Kế hoạch

        public Dictionary<int, int> DangKyIds { get; set; } //Dùng để truy cập vào trang Chi tiết kế hoạch

        public Dictionary<int, int> TienDoKeHoach { get; set; } //Lặp lại bên Chi tiết kế hoạch
    }
}