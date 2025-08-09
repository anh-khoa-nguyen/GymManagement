// Trong file Models/ViewModels/BaiTapViewModel.cs
using System.Collections.Generic;

namespace GymManagementSystem.Models.ViewModels
{
    public class BaiTapViewModel
    {
        // Đối tượng BaiTap chính để binding
        public BaiTap BaiTap { get; set; }

        // Có thể thêm các danh sách khác ở đây nếu cần (ví dụ: danh sách dụng cụ)
        // public IEnumerable<SelectListItem> DanhSachDungCu { get; set; }

        public BaiTapViewModel()
        {
            // Khởi tạo để tránh lỗi null
            BaiTap = new BaiTap();
            BaiTap.CacBuocThucHien = new List<BuocThucHien>();
        }
    }
}