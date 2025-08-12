using System.Collections.Generic;

namespace GymManagementSystem.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public ApplicationUser UserAccount { get; set; }

        //Lấy từ user hiện tại
        public string UserName { get; set; }
        public string Email { get; set; }

        //Lấy từ user.HoiVien
        public string MaGioiThieu { get; set; }
        public string QrCodeUri { get; set; } //Generate từ ApplicationUserID

        // Lấy từ View Model khác
        public HangThanhVienViewModel MembershipInfo { get; set; }
        public LichSuGioiThieuViewModel ReferralInfo { get; set; }

        public List<KhuyenMai> KhuyenMaiDaNhan { get; set; } // KM của hạng hiện tại
        public List<KhuyenMai> KhuyenMaiSapToi { get; set; } // KM của hạng tiếp theo
        public string TenHangTiepTheo { get; set; }


        public UserProfileViewModel()
        {
            MembershipInfo = new HangThanhVienViewModel();
            ReferralInfo = new LichSuGioiThieuViewModel();
            KhuyenMaiDaNhan = new List<KhuyenMai>();
            KhuyenMaiSapToi = new List<KhuyenMai>();
        }
    }
}