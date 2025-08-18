// Trong file Models/ViewModels/TopItemViewModel.cs
namespace GymManagementSystem.Models.ViewModels
{
    // Lớp này dùng để chứa kết quả thống kê cho các bảng "Top"
    // Ví dụ: Top Gói Tập, Top Kế Hoạch...
    public class TopItemViewModel<T>
    {
        public T Item { get; set; } 
        public int SoLuotDangKy { get; set; }
    }
}