using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using System.Globalization;


namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboard/Index
        public async Task<ActionResult> Index(DateTime? chamCongDate, int? reportYear, int? reportMonth, string chamCongViewType = "Ngay")
        {
            var viewModel = new DashboardViewModel();
            DateTime today = DateTime.Today;

            // --- XỬ LÝ CÁC GIÁ TRỊ BỘ LỌC ---
            viewModel.SelectedReportYear = reportYear ?? today.Year;
            viewModel.SelectedReportMonth = reportMonth;

            viewModel.SelectedChamCongDate = chamCongDate ?? today;
            viewModel.SelectedChamCongViewType = chamCongViewType;

            await LoadStatCards(viewModel);
            await LoadInitialChartData(viewModel);
            await LoadAvailableYears(viewModel);

            viewModel.BangChamCong = await GetChamCongData(viewModel.SelectedChamCongDate, chamCongViewType);
            viewModel.TopGoiTaps = await GetTopGoiTapData(viewModel.SelectedReportYear, viewModel.SelectedReportMonth);
            viewModel.TopKeHoachs = await GetTopKeHoachData(viewModel.SelectedReportYear, viewModel.SelectedReportMonth);
            viewModel.TopPts = await GetTopPtData(viewModel.SelectedReportYear, viewModel.SelectedReportMonth); // <-- GỌI HÀM MỚI

            return View(viewModel);
        }

        #region Load
        private async Task LoadStatCards(DashboardViewModel viewModel)
        {
            DateTime today = DateTime.Today;
            DateTime endOfToday = today.AddDays(1);
            viewModel.TotalVisitorsToday = await db.LichSuCheckins.Where(c => c.ThoiGianCheckin >= today && c.ThoiGianCheckin < endOfToday && c.ThanhCong).GroupBy(c => c.ApplicationUserId).CountAsync();
            viewModel.TotalRevenueToday = await db.HoaDons.Where(h => h.NgayTao >= today && h.NgayTao < endOfToday && h.TrangThai == TrangThai.DaThanhToan).SumAsync(h => (decimal?)h.ThanhTien) ?? 0;
        }
        private async Task LoadInitialChartData(DashboardViewModel viewModel)
        {
            var visitorData = new List<int>();
            var dateLabels = new List<string>();
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Today.AddDays(-i);
                var nextDate = date.AddDays(1);
                var count = await db.LichSuCheckins.Where(c => c.ThoiGianCheckin >= date && c.ThoiGianCheckin < nextDate && c.ThanhCong && c.VaiTro == "HoiVien").GroupBy(c => c.ApplicationUserId).CountAsync();
                visitorData.Add(count);
                dateLabels.Add($"'{date:dd/MM}'");
            }
            viewModel.ChartLabels = string.Join(",", dateLabels);
            viewModel.ChartData = string.Join(",", visitorData);
        }
        private async Task LoadAvailableYears(DashboardViewModel viewModel)
        {
            int firstYear = (await db.LichSuCheckins.AnyAsync()) ? (await db.LichSuCheckins.MinAsync(c => c.ThoiGianCheckin)).Year : DateTime.Today.Year;
            viewModel.AvailableYears = new List<SelectListItem>();
            for (int year = DateTime.Today.Year; year >= firstYear; year--)
            {
                viewModel.AvailableYears.Add(new SelectListItem { Value = year.ToString(), Text = $"Năm {year}" });
            }
        }
        #endregion

        #region GetTop
        private async Task<List<ChamCongItem>> GetChamCongData(DateTime selectedDate, string viewType)
        {
            DateTime startDate, endDate;

            // 1. Xác định khoảng thời gian lọc
            switch (viewType)
            {
                case "Tuan":
                    startDate = selectedDate.Date.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
                    endDate = startDate.AddDays(7);
                    break;
                case "Thang":
                    startDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                    endDate = startDate.AddMonths(1);
                    break;
                default: // Mặc định là "Ngay"
                    startDate = selectedDate.Date;
                    endDate = startDate.AddDays(1);
                    break;
            }

            // 2. Lấy tất cả check-in của nhân viên trong khoảng thời gian đã chọn
            var allCheckinsInRange = await db.LichSuCheckins
                .Include(c => c.ApplicationUser)
                .Where(c => c.ThoiGianCheckin >= startDate && c.ThoiGianCheckin < endDate &&
                            (c.VaiTro == "PT" || c.VaiTro == "QuanLy") && c.ThanhCong)
                .OrderBy(c => c.ThoiGianCheckin)
                .ToListAsync();

            // 3. Nhóm theo nhân viên
            var groupedByEmployee = allCheckinsInRange.GroupBy(c => c.ApplicationUser);
            var bangChamCongResult = new List<ChamCongItem>();

            foreach (var employeeGroup in groupedByEmployee)
            {
                var user = employeeGroup.Key;
                TimeSpan totalWorkTime = TimeSpan.Zero;

                // Nhóm tiếp theo ngày để tính tổng thời gian làm việc
                var groupedByDay = employeeGroup.GroupBy(c => c.ThoiGianCheckin.Date);

                foreach (var dayGroup in groupedByDay)
                {
                    var checkinsForDay = dayGroup.ToList();
                    var checkinVao = checkinsForDay.FirstOrDefault();
                    var checkinRa = checkinsForDay.Count > 1 ? checkinsForDay.LastOrDefault() : null;

                    if (checkinVao != null && checkinRa != null)
                    {
                        totalWorkTime += (checkinRa.ThoiGianCheckin - checkinVao.ThoiGianCheckin);
                    }
                }

                // 4. Tạo đối tượng kết quả
                var chamCongItem = new ChamCongItem
                {
                    HoTen = user.HoTen,
                    VaiTro = user.VaiTro,
                    ThoiGianVao = (viewType == "Ngay") ? employeeGroup.FirstOrDefault()?.ThoiGianCheckin : (DateTime?)null,
                    ThoiGianRa = (viewType == "Ngay" && employeeGroup.Count() > 1) ? employeeGroup.LastOrDefault()?.ThoiGianCheckin : (DateTime?)null,
                    ThoiGianLamViec = $"{(int)totalWorkTime.TotalHours:D2}:{totalWorkTime.Minutes:D2}"
                };
                bangChamCongResult.Add(chamCongItem);
            }

            return bangChamCongResult.OrderBy(c => c.HoTen).ToList();
        }

        private async Task<List<TopItemViewModel<GoiTap>>> GetTopGoiTapData(int year, int? month)
        {
            var query = db.DangKyGoiTaps.Where(d => d.NgayDangKy.Year == year);
            if (month.HasValue)
            {
                query = query.Where(d => d.NgayDangKy.Month == month.Value);
            }
            return await query
                .GroupBy(d => d.GoiTap)
                .Select(g => new TopItemViewModel<GoiTap> { Item = g.Key, SoLuotDangKy = g.Count() })
                .OrderByDescending(x => x.SoLuotDangKy)
                .Take(10)
                .ToListAsync();
        }
        private async Task<List<TopItemViewModel<KeHoach>>> GetTopKeHoachData(int year, int? month)
        {
            var query = db.DangKyKeHoachs.Where(d => d.NgayBatDau.Year == year);
            if (month.HasValue)
            {
                query = query.Where(d => d.NgayBatDau.Month == month.Value);
            }
            return await query
                .GroupBy(d => d.KeHoach)
                .Select(g => new TopItemViewModel<KeHoach> { Item = g.Key, SoLuotDangKy = g.Count() })
                .OrderByDescending(x => x.SoLuotDangKy)
                .Take(10)
                .ToListAsync();
        }

        private async Task<List<TopPtViewModel>> GetTopPtData(int year, int? month)
        {
            // Bắt đầu truy vấn từ bảng LichTap
            var ptStatsQuery = db.LichTaps
                .Include(l => l.HuanLuyenVien.ApplicationUser) // Nạp sẵn thông tin cần thiết
                .Where(l => l.HuanLuyenVienId.HasValue &&
                            l.TrangThai == TrangThaiLichTap.DaHoanThanh &&
                            l.ThoiGianKetThuc.Year == year);

            // Lọc theo tháng nếu có
            if (month.HasValue)
            {
                ptStatsQuery = ptStatsQuery.Where(l => l.ThoiGianKetThuc.Month == month.Value);
            }

            // Nhóm theo Huấn luyện viên và tính toán các chỉ số
            var ptStats = await ptStatsQuery
                .GroupBy(l => l.HuanLuyenVien)
                .Select(g => new
                {
                    Pt = g.Key,
                    SoLich = g.Count(),
                    DiemTrungBinh = g.Average(l => (double?)l.DanhGiaSao)
                })
                .ToListAsync(); // Kéo vào bộ nhớ để xử lý bước lọc cuối cùng

            // Lọc, sắp xếp và lấy top 10
            var topPts = ptStats
                .Where(pt => pt.DiemTrungBinh == null || pt.DiemTrungBinh >= 4.0)
                .OrderByDescending(pt => pt.SoLich)
                .ThenByDescending(pt => pt.DiemTrungBinh)
                .Take(10)
                .Select(pt => new TopPtViewModel
                {
                    HoTen = pt.Pt.ApplicationUser.HoTen,
                    AvatarUrl = pt.Pt.ApplicationUser.AvatarUrl,
                    SoLichDaHoanThanh = pt.SoLich,
                    DiemDanhGiaTrungBinh = pt.DiemTrungBinh
                })
                .ToList();

            return topPts;
        }
        #endregion

        #region GetChart
        [HttpGet]
        public async Task<JsonResult> GetChartData(int? year, int? month, string viewType = "daily")
        {
            int targetYear = year ?? DateTime.Now.Year;
            var labels = new List<string>();
            var data = new List<int>();

            if (!month.HasValue)
            {
                int endMonth = (targetYear == DateTime.Now.Year) ? DateTime.Now.Month : 12;
                for (int m = 1; m <= endMonth; m++)
                {
                    labels.Add($"'Tháng {m}'");
                    var count = await db.LichSuCheckins
                        .Where(c => c.ThoiGianCheckin.Year == targetYear 
                            && c.ThoiGianCheckin.Month == m && c.ThanhCong && c.VaiTro == "HoiVien")
                        .GroupBy(c => c.ApplicationUserId).CountAsync();
                    data.Add(count);
                }
            }
            else
            {
                if (viewType == "overview")
                {
                    labels.AddRange(new[] { "'Tuần 1'", "'Tuần 2'", "'Tuần 3'", "'Tuần 4+'" });
                    data.AddRange(new[] { 0, 0, 0, 0 });
                    var checkinsInMonth = await db.LichSuCheckins
                        .Where(c => c.ThoiGianCheckin.Year == targetYear 
                            && c.ThoiGianCheckin.Month == month.Value 
                            && c.ThanhCong && c.VaiTro == "HoiVien")
                        .ToListAsync();

                    var dailyVisitors = checkinsInMonth
                        .GroupBy(c => c.ThoiGianCheckin.Date)
                        .Select(g => 
                            new { Day = g.Key.Day, Count = g.Select(c => c.ApplicationUserId)
                                                            .Distinct().Count() });
                    
                    foreach (var day in dailyVisitors)
                    {
                        if (day.Day <= 7) data[0] += day.Count; 
                        else if (day.Day <= 14) data[1] += day.Count; 
                        else if (day.Day <= 21) data[2] += day.Count; 
                        else data[3] += day.Count;
                    }
                }
                else
                {
                    int daysInMonth = DateTime.DaysInMonth(targetYear, month.Value);
                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        var date = new DateTime(targetYear, month.Value, d);
                        labels.Add($"'{date:dd/MM}'");
                        var nextDate = date.AddDays(1);
                        var count = await db.LichSuCheckins
                            .Where(c => c.ThoiGianCheckin >= date.Date 
                                && c.ThoiGianCheckin < nextDate && c.ThanhCong && c.VaiTro == "HoiVien")
                            .GroupBy(c => c.ApplicationUserId).CountAsync();
                        data.Add(count);
                    }
                }
            }
            return Json(new { labels = string.Join(",", labels), data = string.Join(",", data) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetRevenueChartData(int? year, int? month, string viewType = "daily")
        {
            int targetYear = year ?? DateTime.Now.Year;
            var labels = new List<string>();
            var data = new List<decimal>();

            if (!month.HasValue)
            {
                int endMonth = (targetYear == DateTime.Now.Year) ? DateTime.Now.Month : 12;
                for (int m = 1; m <= endMonth; m++)
                {
                    labels.Add($"'Tháng {m}'");
                    var revenue = await db.HoaDons
                        .Where(h => h.NgayTao.Year == targetYear 
                                && h.NgayTao.Month == m && h.TrangThai == TrangThai.DaThanhToan)
                        .SumAsync(h => (decimal?)h.ThanhTien) ?? 0;
                    data.Add(revenue);
                }
            }
            else
            {
                if (viewType == "overview")
                {
                    labels.AddRange(new[] { "'Tuần 1'", "'Tuần 2'", "'Tuần 3'", "'Tuần 4+'" });
                    data.AddRange(new[] { 0m, 0m, 0m, 0m });
                    var invoicesInMonth = 
                        await db.HoaDons
                        .Where(h => h.NgayTao.Year == targetYear && h.NgayTao.Month == month.Value && h.TrangThai == TrangThai.DaThanhToan)
                        .ToListAsync();
                    
                    foreach (var invoice in invoicesInMonth)
                    {
                        if (invoice.NgayTao.Day <= 7) data[0] += invoice.ThanhTien; else if (invoice.NgayTao.Day <= 14) data[1] += invoice.ThanhTien; else if (invoice.NgayTao.Day <= 21) data[2] += invoice.ThanhTien; else data[3] += invoice.ThanhTien;
                    }
                }
                else
                {
                    int daysInMonth = DateTime.DaysInMonth(targetYear, month.Value);
                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        var date = new DateTime(targetYear, month.Value, d);
                        labels.Add($"'{date:dd/MM}'");
                        var nextDate = date.AddDays(1);
                        var revenue = await db.HoaDons
                            .Where(h => h.NgayTao >= date.Date && h.NgayTao < nextDate && h.TrangThai == TrangThai.DaThanhToan)
                            .SumAsync(h => (decimal?)h.ThanhTien) ?? 0;
                        data.Add(revenue);
                    }
                }
            }
            return Json(new { labels = string.Join(",", labels), data = string.Join(",", data) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetStatCardData(int? year, int? month)
        {
            DateTime startDate;
            DateTime endDate;
            string periodLabel;
            int targetYear = year ?? DateTime.Now.Year;

            if (month.HasValue)
            {
                startDate = new DateTime(targetYear, month.Value, 1);
                endDate = startDate.AddMonths(1);
                periodLabel = $"trong tháng {month.Value}/{targetYear}";
            }
            else
            {
                startDate = new DateTime(targetYear, 1, 1);
                endDate = startDate.AddYears(1);
                periodLabel = $"trong năm {targetYear}";
            }

            var totalVisitors = await db.LichSuCheckins
                .Where(c => c.ThoiGianCheckin >= startDate && c.ThoiGianCheckin < endDate && c.ThanhCong)
                .GroupBy(c => c.ApplicationUserId).CountAsync();
            var totalRevenue = await db.HoaDons
                .Where(h => h.NgayTao >= startDate && h.NgayTao < endDate && h.TrangThai == TrangThai.DaThanhToan)
                .SumAsync(h => (decimal?)h.ThanhTien) ?? 0;

            return Json(new { totalVisitors = totalVisitors, totalRevenue = totalRevenue.ToString("N0"), periodLabel = periodLabel }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}