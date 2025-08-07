namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewModel_Data : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietKeHoaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KeHoachId = c.Int(nullable: false),
                        BaiTapId = c.Int(nullable: false),
                        NgayTrongKeHoach = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaiTaps", t => t.BaiTapId, cascadeDelete: true)
                .ForeignKey("dbo.KeHoaches", t => t.KeHoachId, cascadeDelete: true)
                .Index(t => t.KeHoachId)
                .Index(t => t.BaiTapId);
            
            CreateTable(
                "dbo.KeHoaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenKeHoach = c.String(nullable: false, maxLength: 200),
                        MoTa = c.String(),
                        ThoiGianThucHien = c.Int(nullable: false),
                        KhuyenMaiId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KhuyenMais", t => t.KhuyenMaiId)
                .Index(t => t.KhuyenMaiId);
            
            CreateTable(
                "dbo.KhuyenMais",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenKhuyenMai = c.String(nullable: false, maxLength: 100),
                        MoTa = c.String(),
                        MaKhuyenMai = c.String(nullable: false, maxLength: 50),
                        PhanTramGiamGia = c.Double(nullable: false),
                        SoTienGiamGia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NgayBatDau = c.DateTime(nullable: false),
                        NgayKetThuc = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HangHoiViens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenHang = c.String(nullable: false, maxLength: 50),
                        NguongChiTieu = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DacQuyen = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DangKyKeHoaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoiVienId = c.String(nullable: false, maxLength: 128),
                        KeHoachId = c.Int(nullable: false),
                        NgayBatDau = c.DateTime(nullable: false),
                        NgayHoanThanh = c.DateTime(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.HoiVienId, cascadeDelete: true)
                .ForeignKey("dbo.KeHoaches", t => t.KeHoachId, cascadeDelete: true)
                .Index(t => t.HoiVienId)
                .Index(t => t.KeHoachId);
            
            CreateTable(
                "dbo.HoaDons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoiVienId = c.String(nullable: false, maxLength: 128),
                        GoiTapId = c.Int(nullable: false),
                        KhuyenMaiId = c.Int(),
                        NgayTao = c.DateTime(nullable: false),
                        GiaGoc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SoTienGiam = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ThanhTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TrangThai = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoiTaps", t => t.GoiTapId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.HoiVienId, cascadeDelete: true)
                .ForeignKey("dbo.KhuyenMais", t => t.KhuyenMaiId)
                .Index(t => t.HoiVienId)
                .Index(t => t.GoiTapId)
                .Index(t => t.KhuyenMaiId);
            
            CreateTable(
                "dbo.TienDoBaiTaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DangKyKeHoachId = c.Int(nullable: false),
                        BaiTapId = c.Int(nullable: false),
                        NgayHoanThanh = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaiTaps", t => t.BaiTapId, cascadeDelete: true)
                .ForeignKey("dbo.DangKyKeHoaches", t => t.DangKyKeHoachId, cascadeDelete: true)
                .Index(t => t.DangKyKeHoachId)
                .Index(t => t.BaiTapId);
            
            AddColumn("dbo.AspNetUsers", "HangHoiVienId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "NguoiGioiThieuId", c => c.String());
            CreateIndex("dbo.AspNetUsers", "HangHoiVienId");
            AddForeignKey("dbo.AspNetUsers", "HangHoiVienId", "dbo.HangHoiViens", "Id");

            //// 1. Thêm Hạng Hội Viên (không có phụ thuộc)
            //Sql("INSERT INTO dbo.HangHoiViens (TenHang, NguongChiTieu, DacQuyen) VALUES (N'Đồng', 0, N'Tích điểm cơ bản')");
            //Sql("INSERT INTO dbo.HangHoiViens (TenHang, NguongChiTieu, DacQuyen) VALUES (N'Bạc', 5000000, N'Giảm 10% dịch vụ')");
            //Sql("INSERT INTO dbo.HangHoiViens (TenHang, NguongChiTieu, DacQuyen) VALUES (N'Vàng', 15000000, N'Giảm 30% dịch vụ, miễn phí nước')");

            //// 2. Thêm Khuyến Mãi (không có phụ thuộc)
            //Sql("INSERT INTO dbo.KhuyenMais (TenKhuyenMai, MoTa, MaKhuyenMai, PhanTramGiamGia, SoTienGiamGia, NgayBatDau, NgayKetThuc, IsActive) VALUES (N'Hoàn thành thử thách', N'Giảm 20% cho lần mua gói tập tiếp theo', 'CHALLENGE30', 20.0, 0, GETDATE(), DATEADD(year, 1, GETDATE()), 1)");
            //Sql("INSERT INTO dbo.KhuyenMais (TenKhuyenMai, MoTa, MaKhuyenMai, PhanTramGiamGia, SoTienGiamGia, NgayBatDau, NgayKetThuc, IsActive) VALUES (N'Chào mừng thành viên mới', N'Giảm 10% cho lần mua gói tập đầu tiên', 'WELCOME10', 10.0, 0, GETDATE(), DATEADD(year, 1, GETDATE()), 1)");


            //// 3. Thêm Kế Hoạch (phụ thuộc vào KhuyenMai)
            //// Giả sử KhuyenMai 'CHALLENGE30' có Id = 1. Đây là điểm yếu của cách này.
            //Sql("INSERT INTO dbo.KeHoaches (TenKeHoach, MoTa, ThoiGianThucHien, KhuyenMaiId, IsActive) VALUES (N'30 Ngày Khởi Động', N'Kế hoạch cho người mới bắt đầu', 30, 1, 1)");

            //// 4. Thêm Chi Tiết Kế Hoạch (phụ thuộc vào KeHoach và BaiTap)
            //// Giả sử KeHoach '30 Ngày Khởi Động' có Id = 1 và các bài tập có Id từ 1 đến 3.
            //Sql("INSERT INTO dbo.ChiTietKeHoaches (KeHoachId, BaiTapId, NgayTrongKeHoach) VALUES (1, 1, 1)"); // Ngày 1: Chạy bộ
            //Sql("INSERT INTO dbo.ChiTietKeHoaches (KeHoachId, BaiTapId, NgayTrongKeHoach) VALUES (1, 1, 2)"); // Ngày 2: Đẩy tạ
            //Sql("INSERT INTO dbo.ChiTietKeHoaches (KeHoachId, BaiTapId, NgayTrongKeHoach) VALUES (1, 1, 3)"); // Ngày 3: Squat
                                                                                                             // ... Thêm cho các ngày còn lại nếu muốn
        }

        public override void Down()
        {
            DropForeignKey("dbo.TienDoBaiTaps", "DangKyKeHoachId", "dbo.DangKyKeHoaches");
            DropForeignKey("dbo.TienDoBaiTaps", "BaiTapId", "dbo.BaiTaps");
            DropForeignKey("dbo.HoaDons", "KhuyenMaiId", "dbo.KhuyenMais");
            DropForeignKey("dbo.HoaDons", "HoiVienId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HoaDons", "GoiTapId", "dbo.GoiTaps");
            DropForeignKey("dbo.DangKyKeHoaches", "KeHoachId", "dbo.KeHoaches");
            DropForeignKey("dbo.DangKyKeHoaches", "HoiVienId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "HangHoiVienId", "dbo.HangHoiViens");
            DropForeignKey("dbo.KeHoaches", "KhuyenMaiId", "dbo.KhuyenMais");
            DropForeignKey("dbo.ChiTietKeHoaches", "KeHoachId", "dbo.KeHoaches");
            DropForeignKey("dbo.ChiTietKeHoaches", "BaiTapId", "dbo.BaiTaps");
            DropIndex("dbo.TienDoBaiTaps", new[] { "BaiTapId" });
            DropIndex("dbo.TienDoBaiTaps", new[] { "DangKyKeHoachId" });
            DropIndex("dbo.HoaDons", new[] { "KhuyenMaiId" });
            DropIndex("dbo.HoaDons", new[] { "GoiTapId" });
            DropIndex("dbo.HoaDons", new[] { "HoiVienId" });
            DropIndex("dbo.DangKyKeHoaches", new[] { "KeHoachId" });
            DropIndex("dbo.DangKyKeHoaches", new[] { "HoiVienId" });
            DropIndex("dbo.AspNetUsers", new[] { "HangHoiVienId" });
            DropIndex("dbo.KeHoaches", new[] { "KhuyenMaiId" });
            DropIndex("dbo.ChiTietKeHoaches", new[] { "BaiTapId" });
            DropIndex("dbo.ChiTietKeHoaches", new[] { "KeHoachId" });
            DropColumn("dbo.AspNetUsers", "NguoiGioiThieuId");
            DropColumn("dbo.AspNetUsers", "HangHoiVienId");
            DropTable("dbo.TienDoBaiTaps");
            DropTable("dbo.HoaDons");
            DropTable("dbo.DangKyKeHoaches");
            DropTable("dbo.HangHoiViens");
            DropTable("dbo.KhuyenMais");
            DropTable("dbo.KeHoaches");
            DropTable("dbo.ChiTietKeHoaches");
        }
    }
}
