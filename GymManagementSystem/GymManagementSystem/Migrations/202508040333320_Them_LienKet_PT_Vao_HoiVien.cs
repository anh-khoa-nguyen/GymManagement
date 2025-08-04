namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Them_LienKet_PT_Vao_HoiVien : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LichTaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoiVienId = c.Int(nullable: false),
                        HuanLuyenVienId = c.Int(),
                        ThoiGianBatDau = c.DateTime(nullable: false),
                        ThoiGianKetThuc = c.DateTime(nullable: false),
                        TrangThai = c.Int(nullable: false),
                        GhiChuHoiVien = c.String(),
                        GhiChuPT = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HoiViens", t => t.HoiVienId, cascadeDelete: true)
                .ForeignKey("dbo.HuanLuyenViens", t => t.HuanLuyenVienId)
                .Index(t => t.HoiVienId)
                .Index(t => t.HuanLuyenVienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LichTaps", "HuanLuyenVienId", "dbo.HuanLuyenViens");
            DropForeignKey("dbo.LichTaps", "HoiVienId", "dbo.HoiViens");
            DropIndex("dbo.LichTaps", new[] { "HuanLuyenVienId" });
            DropIndex("dbo.LichTaps", new[] { "HoiVienId" });
            DropTable("dbo.LichTaps");
        }
    }
}
