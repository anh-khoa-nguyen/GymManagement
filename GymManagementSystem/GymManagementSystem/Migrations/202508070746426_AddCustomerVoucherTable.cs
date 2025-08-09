namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerVoucherTable : DbMigration
    {
        public override void Up()
        {   
        
            CreateTable(
                "dbo.KhuyenMaiCuaHoiViens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoiVienId = c.Int(nullable: false),
                        KhuyenMaiId = c.Int(nullable: false),
                        NgayNhan = c.DateTime(nullable: false),
                        NgayHetHan = c.DateTime(nullable: false),
                        TrangThai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HoiViens", t => t.HoiVienId, cascadeDelete: true)
                .ForeignKey("dbo.KhuyenMais", t => t.KhuyenMaiId, cascadeDelete: true)
                .Index(t => t.HoiVienId)
                .Index(t => t.KhuyenMaiId);
            
            AlterColumn("dbo.DangKyGoiTaps", "NgayHetHan", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KhuyenMaiCuaHoiViens", "KhuyenMaiId", "dbo.KhuyenMais");
            DropForeignKey("dbo.KhuyenMaiCuaHoiViens", "HoiVienId", "dbo.HoiViens");
            DropIndex("dbo.KhuyenMaiCuaHoiViens", new[] { "KhuyenMaiId" });
            DropIndex("dbo.KhuyenMaiCuaHoiViens", new[] { "HoiVienId" });
            DropIndex("dbo.ThongBaos", new[] { "ApplicationUserId" });
            DropIndex("dbo.ChiSoSucKhoes", new[] { "HoiVienId" });
            AlterColumn("dbo.DangKyGoiTaps", "NgayHetHan", c => c.DateTime());
            DropTable("dbo.KhuyenMaiCuaHoiViens");
            DropTable("dbo.ThongBaos");
            DropTable("dbo.ChiSoSucKhoes");
        }
    }
}
