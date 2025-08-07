namespace GymManagementSystem.Models.ViewModels
{
    // ViewModel "mẹ" chứa tất cả thông tin cho trang quản lý tài khoản
    public class UserProfileViewModel
    {
        // Thông tin cơ bản
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MaGioiThieu { get; set; }
        public string QrCodeUri { get; set; }

        // Chứa thông tin từ ViewModel Hạng thành viên
        public HangThanhVienViewModel MembershipInfo { get; set; }

        // Chứa thông tin từ ViewModel Lịch sử giới thiệu
        public LichSuGioiThieuViewModel ReferralInfo { get; set; }

        public UserProfileViewModel()
        {
            // Khởi tạo để tránh lỗi null
            MembershipInfo = new HangThanhVienViewModel();
            ReferralInfo = new LichSuGioiThieuViewModel();
        }
    }
}