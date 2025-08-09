namespace GymManagementSystem.Models.ViewModels
{
    public class HangThanhVienViewModel
    {
        public string TenHoiVien { get; set; } //Lấy từ User login hiện tại

        public decimal TongChiTieu { get; set; } //Tính toán: SUM(HoaDon.Total)

        public HangHoiVien HangHienTai { get; set; } //Tính toán: SUM(HoaDon.Total) --> Hạng

        public HangHoiVien HangTiepTheo { get; set; } 

        public decimal ChiTieuCanThem { get; set; } //So sánh: SUM(HoaDon.Total) cách Hạng kế tiếp

        public int PhanTramHoanThanh { get; set; } 
        // % giữa Tiền(Hạng mới) - Tiền(Hạng cũ) --> 100% --> Chênh lệch --> 100% - x%
    }
}