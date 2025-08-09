using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Models.ViewModels
{
    public class NguoiDuocGioiThieuItem
    {
        public string HoTen { get; set; }
        public string Email { get; set; }
        public System.DateTime NgayThamGia { get; set; }
    }

    public class LichSuGioiThieuViewModel
    {
        //Đếm người nhập mã giới thiệu trùng với mã giới thiệu user hiện tại
        public int TongSoNguoiDaGioiThieu { get; set; } 
        public List<NguoiDuocGioiThieuItem> DanhSachNguoiDuocGioiThieu { get; set; }

        public Dictionary<int, string> CacMocThuong { get; set; }
        public int MocThuongTiepTheo { get; set; }
        public int SoNguoiCanThem { get; set; }
    }
}