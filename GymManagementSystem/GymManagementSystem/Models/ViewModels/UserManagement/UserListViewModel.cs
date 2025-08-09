
namespace GymManagementSystem.Models.ViewModels.UserManagement
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string VaiTro { get; set; }
        public bool IsLockedOut { get; set; } // Cho biết tài khoản có bị khóa không
    }
}