using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Models
{
    public class HuanLuyenVien
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string ChuyenMon { get; set; } 

        [StringLength(1000)]
        public string KinhNghiem { get; set; }

        // Foreign Key tới bảng AspNetUsers
        [Required]
        public string ApplicationUserId { get; set; }

        // Navigation property
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<LichTap> LichTaps { get; set; }

    }
}