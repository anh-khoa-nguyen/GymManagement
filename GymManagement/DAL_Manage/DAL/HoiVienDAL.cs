using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL_Manage.Models; // Để sử dụng được các lớp HoiVien, GymModel...

namespace DAL_Manage.DAL
{
    /// <summary>
    /// Lớp xử lý truy cập dữ liệu cho bảng HoiVien
    /// </summary>
    public class HoiVienDAL
    {
        // === CÁC PHƯƠNG THỨC CRUD CƠ BẢN (THEO MẪU) ===

        /// <summary>
        /// Lấy tất cả hội viên từ cơ sở dữ liệu.
        /// </summary>
        /// <returns>Danh sách các hội viên.</returns>
        public List<HoiVien> GetAll()
        {
            using (var db = new GymModel())
            {
                return db.HoiVien.ToList();
            }
        }

        /// <summary>
        /// Lấy một hội viên theo ID (khóa chính).
        /// </summary>
        /// <param name="id">ID của hội viên cần tìm.</param>
        /// <returns>Đối tượng HoiVien hoặc null nếu không tìm thấy.</returns>
        public HoiVien GetById(int id)
        {
            using (var db = new GymModel())
            {
                return db.HoiVien.Find(id);
            }
        }

        /// <summary>
        /// Thêm một hội viên mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="hoiVien">Đối tượng hội viên cần thêm.</param>
        public void Add(HoiVien hoiVien)
        {
            using (var db = new GymModel())
            {
                db.HoiVien.Add(hoiVien);
                db.SaveChanges(); // Lưu thay đổi
            }
        }

        /// <summary>
        /// Cập nhật thông tin của một hội viên.
        /// </summary>
        /// <param name="hoiVien">Đối tượng hội viên với thông tin đã được cập nhật.</param>
        public void Update(HoiVien hoiVien)
        {
            using (var db = new GymModel())
            {
                db.Entry(hoiVien).State = EntityState.Modified;
                db.SaveChanges(); // Lưu thay đổi
            }
        }

        /// <summary>
        /// Xóa một hội viên khỏi cơ sở dữ liệu theo ID.
        /// </summary>
        /// <param name="id">ID của hội viên cần xóa.</param>
        public void Delete(int id)
        {
            using (var db = new GymModel())
            {
                var hoiVien = db.HoiVien.Find(id);
                if (hoiVien != null)
                {
                    db.HoiVien.Remove(hoiVien);
                    db.SaveChanges(); // Lưu thay đổi
                }
            }
        }

        // === CÁC PHƯƠNG THỨC BỔ SUNG CHO NGHIỆP VỤ ĐĂNG NHẬP/ĐĂNG KÝ ===

        /// <summary>
        /// Tìm một hội viên theo số điện thoại (dùng cho đăng nhập và kiểm tra trùng lặp).
        /// </summary>
        /// <param name="sdt">Số điện thoại cần tìm.</param>
        /// <returns>Đối tượng HoiVien hoặc null nếu không tìm thấy.</returns>
        public HoiVien GetBySDT(string sdt)
        {
            using (var db = new GymModel())
            {
                // FirstOrDefault là cách tốt nhất để tìm một bản ghi duy nhất theo một điều kiện nào đó.
                // Nó sẽ trả về null nếu không tìm thấy, thay vì gây ra lỗi.
                return db.HoiVien.FirstOrDefault(hv => hv.SDT == sdt);
            }
        }

        /// <summary>
        /// (Tùy chọn) Tìm một hội viên theo email (dùng để kiểm tra trùng lặp khi đăng ký).
        /// </summary>
        /// <param name="email">Email cần tìm.</param>
        /// <returns>Đối tượng HoiVien hoặc null nếu không tìm thấy.</returns>
        public HoiVien GetByEmail(string email)
        {
            using (var db = new GymModel())
            {
                return db.HoiVien.FirstOrDefault(hv => hv.Email == email);
            }
        }
    }
}