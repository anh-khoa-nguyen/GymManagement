using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    public class HoiVien
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double ChieuCao { get; set; } 

        [Required]
        public double CanNang { get; set; } 

        [StringLength(200)]
        public string MucTieuTapLuyen { get; set; }

        // Foreign Key tới bảng AspNetUsers
        [Required]
        public string ApplicationUserId { get; set; }

        // Navigation properties
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<DangKyGoiTap> DangKyGoiTaps { get; set; }
        public virtual ICollection<LichTap> LichTaps { get; set; }


    }
}