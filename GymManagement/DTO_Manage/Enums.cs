using System.ComponentModel;
namespace DTO_Manage
{
    public enum TinhTrangThietBi
    {
        [Description("Tốt")] Tot = 0,
        [Description("Đang bảo trì")] DangBaoTri = 1,
        [Description("Hỏng")] Hong = 2
    }
}