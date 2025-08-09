using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    public class KeHoach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string TenKeHoach { get; set; }

        public string MoTa { get; set; }

        public int ThoiGianThucHien { get; set; }

        public int? KhuyenMaiId { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Ảnh Bìa")]
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; }

        [ForeignKey("KhuyenMaiId")]
        public virtual KhuyenMai KhuyenMai { get; set; }

        public virtual ICollection<ChiTietKeHoach> ChiTietKeHoachs { get; set; }
    }
}