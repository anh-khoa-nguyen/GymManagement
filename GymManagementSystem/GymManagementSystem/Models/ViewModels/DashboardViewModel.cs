// Trong Models/ViewModels, tạo file DashboardViewModel.cs
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace GymManagementSystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalVisitorsToday { get; set; }
        public decimal TotalRevenueToday { get; set; }
        public int NewMembersToday { get; set; }

        public string ChartLabels { get; set; }
        public string ChartData { get; set; }

        public List<ChamCongItem> BangChamCong { get; set; }
        public DateTime SelectedChamCongDate { get; set; }
        public string SelectedChamCongViewType { get; set; } // Sẽ có giá trị "Ngay", "Tuan", "Thang"


        public List<SelectListItem> AvailableYears { get; set; }
        public int SelectedYear { get; set; }


        public List<TopItemViewModel<GoiTap>> TopGoiTaps { get; set; }
        public List<TopItemViewModel<KeHoach>> TopKeHoachs { get; set; }
        public List<TopPtViewModel> TopPts { get; set; }

        public int SelectedReportYear { get; set; }
        public int? SelectedReportMonth { get; set; }

        public DashboardViewModel()
        {
            BangChamCong = new List<ChamCongItem>();
            TopGoiTaps = new List<TopItemViewModel<GoiTap>>();
            TopKeHoachs = new List<TopItemViewModel<KeHoach>>();
            TopPts = new List<TopPtViewModel>();
        }
    }

    public class ChamCongItem
    {
        public string HoTen { get; set; }
        public string VaiTro { get; set; }
        public DateTime? ThoiGianVao { get; set; }
        public DateTime? ThoiGianRa { get; set; }
        public string ThoiGianLamViec { get; set; } // Dạng chuỗi "HH:mm"
    }

    public class TopPtViewModel
    {
        public string HoTen { get; set; }
        public string AvatarUrl { get; set; }
        public int SoLichDaHoanThanh { get; set; }
        public double? DiemDanhGiaTrungBinh { get; set; }
    }
}