// Trong file Models/LichSuCheckin.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GymManagementSystem.Models;

public class LichSuCheckin
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    [Required]
    [StringLength(50)]
    public string VaiTro { get; set; }

    [Required]
    public DateTime ThoiGianCheckin { get; set; }

    [Required]
    public bool ThanhCong { get; set; }

    [StringLength(200)]
    public string GhiChu { get; set; }

    [ForeignKey("ApplicationUserId")]
    public virtual ApplicationUser ApplicationUser { get; set; }
}