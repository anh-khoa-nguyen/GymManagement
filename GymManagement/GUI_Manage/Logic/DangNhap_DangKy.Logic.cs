using System;
using System.Windows.Forms;
using DTO_Manage;
using BUS_Manage;
using MaterialSkin;
using MaterialSkin.Controls;

namespace GUI_Manage // Giữ cùng namespace với file .cs chính
{
    public partial class DangNhap_DangKy : MaterialForm
    {
        #region Xử lý nghiệp vụ (Business Logic)

        /// <summary>
        /// Xử lý sự kiện khi người dùng nhấn nút Đăng nhập.
        /// </summary>
        private void HandleLogin()
        {
            // 1. Lấy dữ liệu từ form và tạo DTO
            var loginDto = new DangNhapDTO
            {
                SDT = txtLoginSDT.Text.Trim(),
                Password = txtLoginPassword.Text
            };

            // 2. Kiểm tra dữ liệu đầu vào cơ bản
            if (string.IsNullOrEmpty(loginDto.SDT) || string.IsNullOrEmpty(loginDto.Password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ số điện thoại và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Gọi BUS để xử lý
            HoiVienDTO loggedInUser = this.hoiVienBUS.DangNhap(loginDto);

            // 4. Xử lý kết quả trả về
            if (loggedInUser != null)
            {
                MessageBox.Show($"Chào mừng '{loggedInUser.Ten}' đã quay trở lại!", "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Gán thông tin người dùng đã đăng nhập vào thuộc tính public
                this.LoggedInUser = loggedInUser;

                // Đặt kết quả của Dialog là OK và đóng form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Số điện thoại hoặc mật khẩu không chính xác.", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleRegistration()
        {
            // 1. Lấy dữ liệu và tạo DTO
            var registerDto = new DangKyDTO
            {
                Ten = txtRegisterTen.Text.Trim(),
                SDT = txtRegisterSDT.Text.Trim(),
                Email = txtRegisterEmail.Text.Trim(),
                Password = txtRegisterPassword.Text
            };

            // 2. Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(registerDto.Ten) ||
                string.IsNullOrWhiteSpace(registerDto.SDT) ||
                string.IsNullOrWhiteSpace(registerDto.Password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các trường bắt buộc (Họ tên, SĐT, Mật khẩu).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.hoiVienBUS.DangKy(registerDto))
            {
                MessageBox.Show("Đăng ký tài khoản thành công! Bây giờ bạn có thể đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRegisterTen.Clear();
                txtRegisterSDT.Clear();
                txtRegisterEmail.Clear();
                txtRegisterPassword.Clear();
                // Có thể tự động chuyển sang tab đăng nhập
                // tabControl1.SelectedTab = tabPageLogin; 
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại. Số điện thoại này có thể đã được sử dụng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}