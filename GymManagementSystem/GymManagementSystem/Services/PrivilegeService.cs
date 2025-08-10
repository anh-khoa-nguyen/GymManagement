// Trong file Services/PrivilegeService.cs
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GymManagementSystem.Models;

public class PrivilegeService
{
    private readonly ApplicationDbContext _db;

    public PrivilegeService(ApplicationDbContext context)
    {
        _db = context;
    }

    // Hàm này được gọi khi một hội viên được xác nhận lên hạng mới
    public async Task GrantPrivilegesOnRankUp(string applicationUserId, int newRankId)
    {
        var hoivienProfile = await _db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == applicationUserId);
        if (hoivienProfile == null) return;

        // Lấy danh sách các đặc quyền của hạng mới
        var privilegesToGrant = await _db.HangHoiVien_DacQuyens
                                         .Where(hd => hd.HangHoiVienId == newRankId)
                                         .Select(hd => hd.DacQuyen)
                                         .ToListAsync();

        foreach (var dacQuyen in privilegesToGrant)
        {
            // Logic "tặng" đặc quyền.
            // Ví dụ: Tạo một thông báo cho người dùng
            var thongBao = new ThongBao
            {
                ApplicationUserId = applicationUserId,
                NoiDung = $"Chúc mừng! Bạn đã mở khóa đặc quyền mới: '{dacQuyen.TenDacQuyen}'.",
                URL = "/Manage/Index", // Link đến trang profile
                NgayTao = System.DateTime.Now,
                DaXem = false
            };
            _db.ThongBaos.Add(thongBao);

            // Nếu đặc quyền là "Tặng 1 buổi PT", bạn có thể thêm logic ở đây
            // để cộng thêm số buổi vào một trường nào đó của HoiVien.
        }

        await _db.SaveChangesAsync();
    }
}