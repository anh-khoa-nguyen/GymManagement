// Trong file Helpers/HistoryHelper.cs
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GymManagementSystem.Models;
using Newtonsoft.Json;

public static class HistoryHelper
{
    public static string GenerateChangeDescription(LichSuThietBi log)
    {
        if (log.HanhDong == LoaiHanhDong.TaoMoi && !string.IsNullOrEmpty(log.TrangThaiSau))
        {
            var newState = JsonConvert.DeserializeObject<ThietBi>(log.TrangThaiSau);
            return $"Tạo mới thiết bị '{newState.TenThietBi}'.";
        }

        if (log.HanhDong == LoaiHanhDong.Xoa && !string.IsNullOrEmpty(log.TrangThaiTruoc))
        {
            var oldState = JsonConvert.DeserializeObject<ThietBi>(log.TrangThaiTruoc);
            return $"Xóa thiết bị '{oldState.TenThietBi}'.";
        }

        if (log.HanhDong == LoaiHanhDong.CapNhat && !string.IsNullOrEmpty(log.TrangThaiTruoc) && !string.IsNullOrEmpty(log.TrangThaiSau))
        {
            var oldState = JsonConvert.DeserializeObject<ThietBi>(log.TrangThaiTruoc);
            var newState = JsonConvert.DeserializeObject<ThietBi>(log.TrangThaiSau);
            var changes = new List<string>();

            if (oldState.TenThietBi != newState.TenThietBi)
                changes.Add($"tên từ '{oldState.TenThietBi}' thành '{newState.TenThietBi}'");
            if (oldState.MoTa != newState.MoTa)
                changes.Add("mô tả");
            if (oldState.NgayMua != newState.NgayMua)
                changes.Add($"ngày mua từ '{oldState.NgayMua:dd/MM/yyyy}' thành '{newState.NgayMua:dd/MM/yyyy}'");
            if (oldState.TinhTrang != newState.TinhTrang)
                changes.Add($"tình trạng từ '{oldState.TinhTrang}' thành '{newState.TinhTrang}'");
            if (oldState.PhongId != newState.PhongId)
                changes.Add("phòng");

            if (changes.Any())
            {
                return $"Cập nhật {string.Join(", ", changes)}.";
            }
            return "Cập nhật thông tin (không có thay đổi đáng kể).";
        }

        return "Không có thông tin chi tiết.";
    }
}