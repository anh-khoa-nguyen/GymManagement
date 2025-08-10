using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models.ViewModels.UserManagement {
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required, StringLength(100), Display(Name = "Họ và Tên")]
        public string HoTen { get; set; }

        [Required, Display(Name = "Vai trò")]
        public string VaiTro { get; set; }

        [Display(Name = "Email (Không thể thay đổi)")]
        public string Email { get; set; }
        public string AvatarUrl { get; set; }

        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password), Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password), Display(Name = "Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }
}