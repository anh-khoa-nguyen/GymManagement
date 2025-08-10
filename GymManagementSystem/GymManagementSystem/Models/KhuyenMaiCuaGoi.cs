using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{
    public class KhuyenMaiCuaGoi
    {
        [Key, Column(Order = 0)]
        public int KhuyenMaiId { get; set; }

        [Key, Column(Order = 1)]
        public int GoiTapId { get; set; }

        [ForeignKey("KhuyenMaiId")]
        public virtual KhuyenMai KhuyenMai { get; set; }

        [ForeignKey("GoiTapId")]
        public virtual GoiTap GoiTap { get; set; }
    }
}