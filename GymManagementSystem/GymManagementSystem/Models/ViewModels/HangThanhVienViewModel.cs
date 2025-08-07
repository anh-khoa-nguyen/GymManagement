namespace GymManagementSystem.Models.ViewModels
{
    public class HangThanhVienViewModel
    {
        public string TenHoiVien { get; set; }

        public HangHoiVien HangHienTai { get; set; }

        public HangHoiVien HangTiepTheo { get; set; }

        public decimal TongChiTieu { get; set; }
        public decimal ChiTieuCanThem { get; set; }
        public int PhanTramHoanThanh { get; set; } // % để vẽ thanh tiến trình
    }
}