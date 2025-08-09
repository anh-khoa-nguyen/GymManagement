using System.Collections.Generic;

namespace GymManagementSystem.Models.ViewModels
{
    public class GoiTapViewModel
    {
        public IEnumerable<DangKyGoiTap> GoiTapDaDangKy { get; set; } //Model: Đăng kí gói tập

        public IEnumerable<GoiTap> GoiTapChuaDangKy { get; set; } //Model: Gói tập (Lấy toàn bộ)
    }
}