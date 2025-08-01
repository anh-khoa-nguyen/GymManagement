using DAL_Manage.DAL;
using DAL_Manage.Models;
using DTO_Manage;
using System;
using System.Security.Cryptography;
using System.Text;


namespace BUS_Manage
{
    public class HoiVienBUS
    {
        private HoiVienDAL hoiVienDAL = new HoiVienDAL();

        #region Logic Đăng nhập bằng SDT (Đã cập nhật)

        public HoiVienDTO DangNhap(DangNhapDTO dto)
        {
            if (string.IsNullOrEmpty(dto.SDT) || string.IsNullOrEmpty(dto.Password))
            {
                return null;
            }

            // 1. Tìm hội viên theo SĐT trong CSDL
            var hoiVienEntity = hoiVienDAL.GetBySDT(dto.SDT);
            if (hoiVienEntity == null)
            {
                return null; // Không tìm thấy người dùng với SĐT này
            }

            // 2. Xác thực mật khẩu
            if (VerifyPassword(dto.Password, hoiVienEntity.PasswordHash))
            {
                // Đăng nhập thành công, trả về thông tin hội viên
                return new HoiVienDTO
                {
                    ID = hoiVienEntity.ID,
                    Ten = hoiVienEntity.Ten,
                    Email = hoiVienEntity.Email,
                    SDT = hoiVienEntity.SDT
                };
            }

            return null; // Sai mật khẩu
        }

        #endregion

        #region Logic Đăng ký (Đã cập nhật)

        public bool DangKy(DangKyDTO dto)
        {
            // 1. Kiểm tra xem SĐT đã tồn tại chưa
            if (hoiVienDAL.GetBySDT(dto.SDT) != null)
            {
                return false; // Số điện thoại đã được sử dụng
            }

            // (Tùy chọn) Kiểm tra email nếu bạn vẫn muốn email là duy nhất
            // if (hoiVienDAL.GetByEmail(dto.Email) != null) { return false; }

            // 2. Băm mật khẩu
            string passwordHash = HashPassword(dto.Password);

            // 3. Tạo Entity để lưu
            var newHoiVien = new HoiVien
            {
                Ten = dto.Ten,
                Email = dto.Email,
                SDT = dto.SDT,
                PasswordHash = passwordHash
            };

            // 4. Gọi DAL để thêm
            try
            {
                hoiVienDAL.Add(newHoiVien);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        // ... Các hàm băm mật khẩu giữ nguyên ...
        #region Tiện ích xử lý Mật khẩu (Password Hashing)

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            string hashOfInput = HashPassword(password);
            return StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, storedHash) == 0;
        }

        #endregion
    }
}