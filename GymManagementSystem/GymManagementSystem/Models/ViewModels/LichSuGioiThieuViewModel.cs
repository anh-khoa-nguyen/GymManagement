using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Models.ViewModels
{
    // Lớp này chứa thông tin rút gọn của một người được giới thiệu
    public class NguoiDuocGioiThieuItem
    {
        public string HoTen { get; set; }
        public string Email { get; set; }
        public System.DateTime NgayThamGia { get; set; }
    }

    // ViewModel chính cho cả trang
    public class LichSuGioiThieuViewModel
    {
        public int TongSoNguoiDaGioiThieu { get; set; }
        public List<NguoiDuocGioiThieuItem> DanhSachNguoiDuocGioiThieu { get; set; }

        // Thông tin về các mốc thưởng để hiển thị cho người dùng
        public Dictionary<int, string> CacMocThuong { get; set; }
        public int MocThuongTiepTheo { get; set; }
        public int SoNguoiCanThem { get; set; }
    }
}