using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GymManagementSystem.Models;
using System.Data.Entity;

namespace GymManagementSystem.Services
{
    public class BookingService
    {
        private readonly ApplicationDbContext _db;

        public BookingService(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<BookingResult> CreateNewBookingAsync(int hoiVienId, int ptId, DateTime startTime, string notes)
        {
            // Sử dụng transaction để đảm bảo toàn vẹn dữ liệu
            // Hoặc tạo lịch thành công và trừ buổi tập thành công, hoặc không làm gì cả.
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // 1. KIỂM TRA SỐ BUỔI TẬP PT CÒN LẠI CỦA HỘI VIÊN
                    // Tìm gói đăng ký đang hoạt động VÀ còn lượt tập với PT
                    var activeSubscription = await _db.DangKyGoiTaps
                        .Where(d => d.HoiVienId == hoiVienId &&
                                     d.TrangThai == TrangThaiDangKy.HoatDong &&
                                     d.NgayHetHan >= DateTime.Today &&
                                     d.SoBuoiPTDaSuDung < d.SoBuoiTapVoiPT)
                        .OrderByDescending(d => d.NgayHetHan) // Ưu tiên gói nào hết hạn sau cùng
                        .FirstOrDefaultAsync();

                    if (activeSubscription == null)
                    {
                        return new BookingResult { Success = false, Message = "Bạn đã hết số buổi tập với HLV hoặc gói tập của bạn không hỗ trợ tính năng này." };
                    }

                    // 2. KIỂM TRA THỜI GIAN HỢP LỆ
                    if (startTime < DateTime.Now)
                    {
                        return new BookingResult { Success = false, Message = "Không thể đặt lịch vào thời gian trong quá khứ." };
                    }
                    var endTime = startTime.AddHours(1); // Giả sử mỗi buổi tập kéo dài 1 tiếng

                    // 3. KIỂM TRA LỊCH CỦA PT (CONFLICT)
                    bool isPtBusy = await _db.LichTaps.AnyAsync(l =>
                        l.HuanLuyenVienId == ptId &&
                        l.TrangThai != TrangThaiLichTap.DaHuy && // Lịch đã hủy thì không tính
                        l.ThoiGianBatDau < endTime && l.ThoiGianKetThuc > startTime); // Công thức kiểm tra 2 khoảng thời gian giao nhau

                    if (isPtBusy)
                    {
                        return new BookingResult { Success = false, Message = "Rất tiếc, HLV đã có lịch vào khung giờ này. Vui lòng chọn giờ khác." };
                    }

                    // 4. KIỂM TRA LỊCH CỦA CHÍNH HỘI VIÊN (CONFLICT)
                    bool isMemberBusy = await _db.LichTaps.AnyAsync(l =>
                        l.HoiVienId == hoiVienId &&
                        l.TrangThai != TrangThaiLichTap.DaHuy &&
                        l.ThoiGianBatDau < endTime && l.ThoiGianKetThuc > startTime);

                    if (isMemberBusy)
                    {
                        return new BookingResult { Success = false, Message = "Bạn đã có một lịch tập khác bị trùng vào khung giờ này." };
                    }

                    // === MỌI THỨ ĐỀU HỢP LỆ ===

                    // 5. TẠO LỊCH HẸN MỚI
                    var newBooking = new LichTap
                    {
                        HoiVienId = hoiVienId,
                        HuanLuyenVienId = ptId,
                        ThoiGianBatDau = startTime,
                        ThoiGianKetThuc = endTime,
                        GhiChuHoiVien = notes,
                        TrangThai = TrangThaiLichTap.ChoDuyet
                    };
                    _db.LichTaps.Add(newBooking);

                    // 6. CẬP NHẬT (TRỪ ĐI) SỐ BUỔI TẬP CỦA HỘI VIÊN
                    activeSubscription.SoBuoiPTDaSuDung++;

                    await _db.SaveChangesAsync();
                    transaction.Commit(); // Lưu tất cả thay đổi vào CSDL

                    return new BookingResult { Success = true, Message = "Yêu cầu đặt lịch đã được gửi đi thành công!" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Hoàn tác tất cả nếu có lỗi
                    // Ghi log lỗi ra hệ thống để debug
                    System.Diagnostics.Debug.WriteLine($"BookingService Error: {ex.ToString()}");
                    return new BookingResult { Success = false, Message = "Đã có lỗi không mong muốn từ hệ thống. Vui lòng thử lại." };
                }
            }
        }
        public async Task<List<TimeSpan>> GetAvailableSlotsAsync(int ptId, DateTime date)
        {
            var workingHoursStart = new TimeSpan(8, 0, 0); // Giờ làm việc bắt đầu từ 8h sáng
            var workingHoursEnd = new TimeSpan(21, 0, 0);   // và kết thúc lúc 9h tối
            var slotDuration = TimeSpan.FromHours(1);       // Mỗi buổi tập kéo dài 1 tiếng

            // Lấy danh sách các giờ đã được đặt của PT trong ngày đó
            var bookedStartTimes = await _db.LichTaps
                .Where(l => l.HuanLuyenVienId == ptId &&
                            DbFunctions.TruncateTime(l.ThoiGianBatDau) == date.Date &&
                            l.TrangThai != TrangThaiLichTap.DaHuy)
                .Select(l => DbFunctions.CreateTime(l.ThoiGianBatDau.Hour, l.ThoiGianBatDau.Minute, l.ThoiGianBatDau.Second))
                .ToListAsync();

            var availableSlots = new List<TimeSpan>();
            var currentTimeSlot = workingHoursStart;

            // Chạy vòng lặp qua tất cả các khung giờ làm việc
            while (currentTimeSlot < workingHoursEnd)
            {
                // Chỉ thêm vào danh sách nếu khung giờ này:
                // 1. Chưa có ai đặt.
                // 2. Nó là một thời điểm trong tương lai (đối với ngày hôm nay).
                if (!bookedStartTimes.Contains(currentTimeSlot) && date.Date.Add(currentTimeSlot) > DateTime.Now)
                {
                    availableSlots.Add(currentTimeSlot);
                }
                currentTimeSlot = currentTimeSlot.Add(slotDuration);
            }
            return availableSlots;
        }
    }
    public class BookingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}