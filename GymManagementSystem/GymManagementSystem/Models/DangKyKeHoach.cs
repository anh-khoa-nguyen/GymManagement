using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymManagementSystem.Models;

namespace GymManagementSystem.Models
{
    public class DangKyKeHoach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HoiVienId { get; set; }

        [Required]
        public int KeHoachId { get; set; }

        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayHoanThanh { get; set; }

        public string TrangThai { get; set; }

        [ForeignKey("HoiVienId")]
        public virtual ApplicationUser HoiVien { get; set; }

        [ForeignKey("KeHoachId")]
        public virtual KeHoach KeHoach { get; set; }
    }
}