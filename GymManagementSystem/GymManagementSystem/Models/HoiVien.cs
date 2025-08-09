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

        [StringLength(6)]
        [Index(IsUnique = true)]
        public string MaGioiThieu { get; set; }
        public int? HangHoiVienId { get; set; } = 1;
        [ForeignKey("HangHoiVienId")]
        public virtual HangHoiVien HangHoiVien { get; set; } 

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<DangKyGoiTap> DangKyGoiTaps { get; set; }
        public virtual ICollection<LichTap> LichTaps { get; set; }
        public virtual ICollection<ChiSoSucKhoe> ChiSoSucKhoes { get; set; }

    }
}