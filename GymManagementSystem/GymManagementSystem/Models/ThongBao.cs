using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    public class ThongBao
    {
        [Key]
        public int Id { get; set; }

        // Khóa ngoại đến người dùng sẽ nhận thông báo
        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        [StringLength(500)]
        public string NoiDung { get; set; }

        // URL để khi click vào thông báo sẽ chuyển đến
        public string URL { get; set; }

        [Required]
        public DateTime NgayTao { get; set; }

        public bool DaXem { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}