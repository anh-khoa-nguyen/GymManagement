// Trong file Helpers/KhuyenMaiHelper.cs
using GymManagementSystem.Models;
using System.Linq;

namespace GymManagementSystem.Helpers
{
    public static class KhuyenMaiHelper
    {
        public class QuyTacApDungInfo
        {
            public string TenQuyTac { get; set; }
            public IQueryable<GoiTap> DanhSachGoiTapLienQuan { get; set; }
        }

        public static QuyTacApDungInfo SuyLuanQuyTacApDung(KhuyenMai khuyenMai, ApplicationDbContext db)
        {
            var result = new QuyTacApDungInfo();

            int totalGoiTapCount = db.GoiTaps.Count();
            int linkedGoiTapCount = khuyenMai.ApDungChoGoiTap.Count;

            if (linkedGoiTapCount == 0)
            {
                result.TenQuyTac = "Áp dụng cho Tất cả gói tập";
                result.DanhSachGoiTapLienQuan = null;
            }
            else
            {
                if (linkedGoiTapCount <= totalGoiTapCount / 2)
                {
                    result.TenQuyTac = "Chỉ áp dụng cho các gói đã chọn";
                    var linkedIds = khuyenMai.ApDungChoGoiTap.Select(g => g.GoiTapId);
                    result.DanhSachGoiTapLienQuan = db.GoiTaps.Where(g => linkedIds.Contains(g.Id));
                }
                else
                {
                    result.TenQuyTac = "Ngoại trừ các gói đã chọn";
                    var includedIds = khuyenMai.ApDungChoGoiTap.Select(g => g.GoiTapId);
                    var excludedIds = db.GoiTaps.Select(g => g.Id).Except(includedIds);
                    result.DanhSachGoiTapLienQuan = db.GoiTaps.Where(g => excludedIds.Contains(g.Id));
                }
            }

            return result;
        }
    }
}
