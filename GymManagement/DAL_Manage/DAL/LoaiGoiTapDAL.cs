using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_Manage.Models; // Để sử dụng được các lớp LoaiGoiTap, GoiTap...

namespace DAL_Manage.DAL
{
    public class LoaiGoiTapDAL
    {
        // 1. Hàm đọc/lấy tất cả các loại gói tập
        public List<LoaiGoiTap> GetAll()
        {
            // Khối lệnh 'using' đảm bảo DbContext sẽ được giải phóng ngay sau khi xong việc
            using (var db = new GymModel()) // <-- THAY THẾ "GymModel" BẰNG TÊN DbContext CỦA BẠN
            {
                // Dùng LINQ để lấy dữ liệu và chuyển thành danh sách
                return db.LoaiGoiTap.ToList();
            }
        }

        // 2. Hàm lấy một loại gói tập theo ID
        public LoaiGoiTap GetById(int id)
        {
            using (var db = new GymModel()) // <-- THAY THẾ "GymModel" BẰNG TÊN DbContext CỦA BẠN
            {
                // Find là cách nhanh nhất để tìm một đối tượng theo khóa chính
                return db.LoaiGoiTap.Find(id);
            }
        }

        // 3. Hàm thêm một loại gói tập mới
        public void Add(LoaiGoiTap loai)
        {
            using (var db = new GymModel()) // <-- THAY THẾ "GymModel" BẰNG TÊN DbContext CỦA BẠN
            {
                // Thêm đối tượng mới vào DbSet
                db.LoaiGoiTap.Add(loai);
                // Gọi SaveChanges() để thực sự lưu các thay đổi xuống cơ sở dữ liệu
                db.SaveChanges();
            }
        }

        // 4. Hàm cập nhật một loại gói tập
        public void Update(LoaiGoiTap loai)
        {
            using (var db = new GymModel()) // <-- THAY THẾ "GymModel" BẰNG TÊN DbContext CỦA BẠN
            {
                // Đính kèm đối tượng vào context và đánh dấu nó là đã bị sửa đổi
                db.Entry(loai).State = EntityState.Modified;
                // Lưu thay đổi
                db.SaveChanges();
            }
        }

        // 5. Hàm xóa một loại gói tập
        public void Delete(int id)
        {
            using (var db = new GymModel()) // <-- THAY THẾ "GymModel" BẰNG TÊN DbContext CỦA BẠN
            {
                // Tìm đối tượng cần xóa
                var loai = db.LoaiGoiTap.Find(id);
                if (loai != null) // Luôn kiểm tra xem đối tượng có tồn tại không
                {
                    // Xóa đối tượng khỏi DbSet
                    db.LoaiGoiTap.Remove(loai);
                    // Lưu thay đổi
                    db.SaveChanges();
                }
            }
        }
    }
}
