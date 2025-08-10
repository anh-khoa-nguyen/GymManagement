using GymManagementSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

public class ThietBiStateViewModel
{
    public ThietBi ThietBi { get; set; }
    public string TenPhong { get; set; }
}

public class HistoryReconstructionService
{
    private readonly ApplicationDbContext _db;
    public HistoryReconstructionService(ApplicationDbContext context) { _db = context; }

    public async Task<List<ThietBiStateViewModel>> GetEquipmentStateAtAsync(DateTime targetDate)
    {
        var currentState = await _db.ThietBis
                                    .AsNoTracking()
                                    .ToDictionaryAsync(t => t.Id);

        DateTime endOfTargetDate = targetDate.Date.AddDays(1);
        var logsToRevert = await _db.LichSuThietBis
            .Where(l => l.NgayThucHien >= endOfTargetDate)
            .OrderByDescending(l => l.NgayThucHien)
            .ToListAsync();

        foreach (var log in logsToRevert)
        {
            switch (log.HanhDong)
            {
                case LoaiHanhDong.TaoMoi:
                    if (currentState.ContainsKey(log.ThietBiId))
                    {
                        currentState.Remove(log.ThietBiId);
                    }
                    break;

                case LoaiHanhDong.CapNhat:
                    if (currentState.ContainsKey(log.ThietBiId) && !string.IsNullOrEmpty(log.TrangThaiTruoc))
                    {
                        currentState[log.ThietBiId] = JsonConvert.DeserializeObject<ThietBi>(log.TrangThaiTruoc);
                    }
                    break;

                case LoaiHanhDong.Xoa:
                    if (!string.IsNullOrEmpty(log.TrangThaiTruoc))
                    {
                        var restoredThietBi = JsonConvert.DeserializeObject<ThietBi>(log.TrangThaiTruoc);
                        currentState[log.ThietBiId] = restoredThietBi;
                    }
                    break;
            }
        }

        var allRooms = await _db.Phongs.ToDictionaryAsync(p => p.Id, p => p.TenPhong);
        var result = new List<ThietBiStateViewModel>();
        foreach (var thietBi in currentState.Values)
        {
            result.Add(new ThietBiStateViewModel
            {
                ThietBi = thietBi,
                TenPhong = thietBi.PhongId.HasValue && allRooms.ContainsKey(thietBi.PhongId.Value)
                           ? allRooms[thietBi.PhongId.Value]
                           : "None"
            });
        }

        return result;
    }
}