namespace DAL_Manage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescribeYourChangeHere : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DangKy",
                c => new
                    {
                        ID_HoiVien = c.Int(nullable: false),
                        ID_Goi_Tap = c.Int(nullable: false),
                        Ngay_Bat_Dau = c.DateTime(storeType: "date"),
                        Ngay_Ket_Thuc = c.DateTime(storeType: "date"),
                        TrangThai = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.ID_HoiVien, t.ID_Goi_Tap })
                .ForeignKey("dbo.GoiTap", t => t.ID_Goi_Tap)
                .ForeignKey("dbo.HoiVien", t => t.ID_HoiVien)
                .Index(t => t.ID_HoiVien)
                .Index(t => t.ID_Goi_Tap);
            
            CreateTable(
                "dbo.GoiTap",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ten = c.String(nullable: false, maxLength: 255),
                        Gia = c.Decimal(nullable: false, precision: 10, scale: 2),
                        So_Buoi_Tap_PT = c.Int(),
                        ID_Loai_Goi_Tap = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LoaiGoiTap", t => t.ID_Loai_Goi_Tap)
                .Index(t => t.ID_Loai_Goi_Tap);
            
            CreateTable(
                "dbo.HoaDon",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ngay_Tao = c.DateTime(nullable: false),
                        Tinh_Trang = c.String(maxLength: 100),
                        Phuong_Thuc_Thanh_Toan = c.String(maxLength: 100),
                        ID_Nhan_Vien = c.Int(),
                        ID_HoiVien = c.Int(),
                        ID_GoiTap = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HoiVien", t => t.ID_HoiVien)
                .ForeignKey("dbo.NhanVien", t => t.ID_Nhan_Vien)
                .ForeignKey("dbo.GoiTap", t => t.ID_GoiTap)
                .Index(t => t.ID_Nhan_Vien)
                .Index(t => t.ID_HoiVien)
                .Index(t => t.ID_GoiTap);
            
            CreateTable(
                "dbo.HoiVien",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ten = c.String(nullable: false, maxLength: 255),
                        Gioi_Tinh = c.String(maxLength: 10),
                        Ngay_Sinh = c.DateTime(storeType: "date"),
                        Dia_Chi = c.String(maxLength: 255),
                        Email = c.String(maxLength: 255, unicode: false),
                        SDT = c.String(maxLength: 20, unicode: false),
                        PasswordHash = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.SDT, unique: true);
            
            CreateTable(
                "dbo.LichTap",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_HoiVien = c.Int(),
                        ID_Nhan_Vien = c.Int(),
                        Ngay_Tap = c.DateTime(),
                        Trang_Thai = c.String(maxLength: 100),
                        Gio_Tap = c.Time(precision: 7),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NhanVien", t => t.ID_Nhan_Vien)
                .ForeignKey("dbo.HoiVien", t => t.ID_HoiVien)
                .Index(t => t.ID_HoiVien)
                .Index(t => t.ID_Nhan_Vien);
            
            CreateTable(
                "dbo.NhanVien",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ten = c.String(nullable: false, maxLength: 255),
                        Gioi_Tinh = c.String(maxLength: 10),
                        Ngay_Sinh = c.DateTime(storeType: "date"),
                        Dia_Chi = c.String(maxLength: 255),
                        SDT = c.String(maxLength: 20, unicode: false),
                        Email = c.String(maxLength: 255, unicode: false),
                        Vai_Tro = c.String(maxLength: 100),
                        ID_Phong = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Phong", t => t.ID_Phong)
                .Index(t => t.ID_Phong);
            
            CreateTable(
                "dbo.PhieuKiemTra",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_Nhan_Vien = c.Int(),
                        ID_Thiet_Bi = c.Int(),
                        Ngay_Kiem_Tra = c.DateTime(),
                        Mo_Ta = c.String(storeType: "ntext"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ThietBi", t => t.ID_Thiet_Bi)
                .ForeignKey("dbo.NhanVien", t => t.ID_Nhan_Vien)
                .Index(t => t.ID_Nhan_Vien)
                .Index(t => t.ID_Thiet_Bi);
            
            CreateTable(
                "dbo.ThietBi",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ten = c.String(nullable: false, maxLength: 255),
                        Tinh_Trang = c.String(maxLength: 255),
                        ID_Phong = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Phong", t => t.ID_Phong)
                .Index(t => t.ID_Phong);
            
            CreateTable(
                "dbo.Phong",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ten = c.String(nullable: false, maxLength: 255),
                        Dia_Chi = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TienDoTapLuyen",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_HoiVien = c.Int(),
                        ID_Nhan_Vien = c.Int(),
                        Ngay_Ghi_Nhan = c.DateTime(storeType: "date"),
                        Can_Nang = c.Decimal(precision: 5, scale: 2),
                        Ti_Le_Mo = c.Decimal(precision: 5, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NhanVien", t => t.ID_Nhan_Vien)
                .ForeignKey("dbo.HoiVien", t => t.ID_HoiVien)
                .Index(t => t.ID_HoiVien)
                .Index(t => t.ID_Nhan_Vien);
            
            CreateTable(
                "dbo.LoaiGoiTap",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ten = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoiTap", "ID_Loai_Goi_Tap", "dbo.LoaiGoiTap");
            DropForeignKey("dbo.HoaDon", "ID_GoiTap", "dbo.GoiTap");
            DropForeignKey("dbo.TienDoTapLuyen", "ID_HoiVien", "dbo.HoiVien");
            DropForeignKey("dbo.LichTap", "ID_HoiVien", "dbo.HoiVien");
            DropForeignKey("dbo.TienDoTapLuyen", "ID_Nhan_Vien", "dbo.NhanVien");
            DropForeignKey("dbo.PhieuKiemTra", "ID_Nhan_Vien", "dbo.NhanVien");
            DropForeignKey("dbo.ThietBi", "ID_Phong", "dbo.Phong");
            DropForeignKey("dbo.NhanVien", "ID_Phong", "dbo.Phong");
            DropForeignKey("dbo.PhieuKiemTra", "ID_Thiet_Bi", "dbo.ThietBi");
            DropForeignKey("dbo.LichTap", "ID_Nhan_Vien", "dbo.NhanVien");
            DropForeignKey("dbo.HoaDon", "ID_Nhan_Vien", "dbo.NhanVien");
            DropForeignKey("dbo.HoaDon", "ID_HoiVien", "dbo.HoiVien");
            DropForeignKey("dbo.DangKy", "ID_HoiVien", "dbo.HoiVien");
            DropForeignKey("dbo.DangKy", "ID_Goi_Tap", "dbo.GoiTap");
            DropIndex("dbo.TienDoTapLuyen", new[] { "ID_Nhan_Vien" });
            DropIndex("dbo.TienDoTapLuyen", new[] { "ID_HoiVien" });
            DropIndex("dbo.ThietBi", new[] { "ID_Phong" });
            DropIndex("dbo.PhieuKiemTra", new[] { "ID_Thiet_Bi" });
            DropIndex("dbo.PhieuKiemTra", new[] { "ID_Nhan_Vien" });
            DropIndex("dbo.NhanVien", new[] { "ID_Phong" });
            DropIndex("dbo.LichTap", new[] { "ID_Nhan_Vien" });
            DropIndex("dbo.LichTap", new[] { "ID_HoiVien" });
            DropIndex("dbo.HoiVien", new[] { "SDT" });
            DropIndex("dbo.HoaDon", new[] { "ID_GoiTap" });
            DropIndex("dbo.HoaDon", new[] { "ID_HoiVien" });
            DropIndex("dbo.HoaDon", new[] { "ID_Nhan_Vien" });
            DropIndex("dbo.GoiTap", new[] { "ID_Loai_Goi_Tap" });
            DropIndex("dbo.DangKy", new[] { "ID_Goi_Tap" });
            DropIndex("dbo.DangKy", new[] { "ID_HoiVien" });
            DropTable("dbo.LoaiGoiTap");
            DropTable("dbo.TienDoTapLuyen");
            DropTable("dbo.Phong");
            DropTable("dbo.ThietBi");
            DropTable("dbo.PhieuKiemTra");
            DropTable("dbo.NhanVien");
            DropTable("dbo.LichTap");
            DropTable("dbo.HoiVien");
            DropTable("dbo.HoaDon");
            DropTable("dbo.GoiTap");
            DropTable("dbo.DangKy");
        }
    }
}
