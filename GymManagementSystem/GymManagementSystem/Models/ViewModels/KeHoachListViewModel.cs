using System.Collections.Generic;

namespace GymManagementSystem.Models.ViewModels
{
    public class KeHoachListViewModel
    {
        // Danh sách các kế hoạch người dùng ĐÃ ĐĂNG KÝ
        public IEnumerable<KeHoach> DaDangKy { get; set; }

        // Danh sách các kế hoạch người dùng CHƯA ĐĂNG KÝ
        public IEnumerable<KeHoach> ChuaDangKy { get; set; }

        // Dictionary để lấy ID của bản ghi đăng ký
        public Dictionary<int, int> DangKyIds { get; set; }

        // Dictionary để lấy phần trăm tiến độ
        public Dictionary<int, int> TienDoKeHoach { get; set; }
    }
}