using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

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
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // 1. KIỂM TRA SỐ BUỔI TẬP PT CÒN LẠI CỦA HỘI VIÊN
                    var activeSubscription = await _db.DangKyGoiTaps
                        .Where(d => d.HoiVienId == hoiVienId &&
                                     d.TrangThai == TrangThaiDangKy.HoatDong &&
                                     d.NgayHetHan >= DateTime.Today &&
                                     d.SoBuoiPTDaSuDung < d.SoBuoiTapVoiPT)
                        .OrderByDescending(d => d.NgayHetHan)
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
                    var endTime = startTime.AddHours(1);

                    // 3. KIỂM TRA LỊCH CỦA PT (CONFLICT)
                    bool isPtBusy = await _db.LichTaps.AnyAsync(l =>
                        l.HuanLuyenVienId == ptId &&
                        l.TrangThai != TrangThaiLichTap.DaHuy &&
                        l.ThoiGianBatDau < endTime && l.ThoiGianKetThuc > startTime);

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

                    // 6. CẬP NHẬT SỐ BUỔI TẬP CỦA HỘI VIÊN
                    activeSubscription.SoBuoiPTDaSuDung++;
                    _db.Entry(activeSubscription).State = EntityState.Modified;

                    // 7. TẠO THÔNG BÁO CHO PT
                    var ptUser = await _db.HuanLuyenViens.Where(pt => pt.Id == ptId).Select(pt => pt.ApplicationUser).FirstOrDefaultAsync();
                    var hoiVienUser = await _db.HoiViens.Where(hv => hv.Id == hoiVienId).Select(hv => hv.ApplicationUser).FirstOrDefaultAsync();

                    ThongBao thongBaoChoPT = null;
                    if (ptUser != null && hoiVienUser != null)
                    {
                        thongBaoChoPT = new ThongBao
                        {
                            ApplicationUserId = ptUser.Id,
                            NoiDung = $"Yêu cầu đặt lịch mới từ {hoiVienUser.HoTen} lúc {startTime:HH:mm dd/MM}.",
                            URL = "#", // URL tạm thời
                            NgayTao = DateTime.Now,
                            DaXem = false
                        };
                        _db.ThongBaos.Add(thongBaoChoPT);
                    }

                    // 8. LƯU LẦN 1
                    await _db.SaveChangesAsync();

                    // 9. CẬP NHẬT URL CHO THÔNG BÁO (NẾU ĐÃ TẠO)
                    if (thongBaoChoPT != null)
                    {
                        // UrlHelper không có ở đây, nên chúng ta tạo link tương đối
                        thongBaoChoPT.URL = $"/Home/RedirectToNotificationUrl?notificationId={thongBaoChoPT.Id}";
                        _db.Entry(thongBaoChoPT).State = EntityState.Modified;
                        await _db.SaveChangesAsync(); // Lưu lần 2
                    }

                    // 10. COMMIT TRANSACTION
                    transaction.Commit();
                    return new BookingResult { Success = true, Message = "Yêu cầu đặt lịch đã được gửi đi thành công!" };
                }
                catch (DbEntityValidationException dbEx)
                {
                    transaction.Rollback();
                    var errorMessages = new List<string>();
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            errorMessages.Add($"Entity: {validationErrors.Entry.Entity.GetType().Name}, Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                        }
                    }
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    System.Diagnostics.Debug.WriteLine($"=== DbEntityValidationException: {fullErrorMessage} ===");
                    return new BookingResult { Success = false, Message = $"Dữ liệu không hợp lệ: {fullErrorMessage}" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine($"BookingService General Error: {ex.ToString()}");
                    return new BookingResult { Success = false, Message = "Đã có lỗi không mong muốn từ hệ thống. Vui lòng thử lại." };
                }
            }
        }

        public async Task<List<string>> GetAvailableSlotsAsync(int ptId, DateTime date)
        {
            var workingHoursStart = new TimeSpan(8, 0, 0);
            var workingHoursEnd = new TimeSpan(21, 0, 0);
            var slotDuration = TimeSpan.FromHours(1);

            var bookedStartTimes = await _db.LichTaps
                .Where(l => l.HuanLuyenVienId == ptId &&
                            DbFunctions.TruncateTime(l.ThoiGianBatDau) == date.Date &&
                            l.TrangThai != TrangThaiLichTap.DaHuy)
                .Select(l => DbFunctions.CreateTime(l.ThoiGianBatDau.Hour, l.ThoiGianBatDau.Minute, l.ThoiGianBatDau.Second))
                .ToListAsync();

            // Chuyển sang HashSet để tìm kiếm nhanh hơn
            var bookedSlots = new HashSet<TimeSpan?>(bookedStartTimes);

            var availableSlots = new List<string>();
            var currentTimeSlot = workingHoursStart;

            while (currentTimeSlot < workingHoursEnd)
            {
                if (!bookedSlots.Contains(currentTimeSlot) && date.Date.Add(currentTimeSlot) > DateTime.Now)
                {
                    availableSlots.Add(currentTimeSlot.ToString(@"hh\:mm"));
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