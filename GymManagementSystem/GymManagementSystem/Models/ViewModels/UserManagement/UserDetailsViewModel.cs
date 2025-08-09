namespace GymManagementSystem.Models.ViewModels.UserManagement
{
    public class UserDetailsViewModel
    {
        // Thông tin từ bảng AspNetUsers
        public ApplicationUser UserAccount { get; set; }

        // Thông tin từ bảng HoiViens (có thể null nếu người dùng không phải là hội viên)
        public HoiVien HoiVienProfile { get; set; }
    }
}