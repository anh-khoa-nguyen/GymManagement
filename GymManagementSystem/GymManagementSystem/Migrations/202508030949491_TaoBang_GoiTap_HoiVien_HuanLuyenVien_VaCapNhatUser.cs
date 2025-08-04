namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaoBang_GoiTap_HoiVien_HuanLuyenVien_VaCapNhatUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HoiViens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChieuCao = c.Double(nullable: false),
                        CanNang = c.Double(nullable: false),
                        MucTieuTapLuyen = c.String(maxLength: 200),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        GoiTapId = c.Int(),
                        NgayHetHanGoiTap = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.GoiTaps", t => t.GoiTapId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.GoiTapId);
            
            CreateTable(
                "dbo.HuanLuyenViens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChuyenMon = c.String(maxLength: 500),
                        KinhNghiem = c.String(maxLength: 1000),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            AddColumn("dbo.GoiTaps", "MoTaQuyenLoi", c => c.String(maxLength: 500));
            AddColumn("dbo.GoiTaps", "SoBuoiTapVoiPT", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "VaiTro", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "HoiVienId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "HuanLuyenVienId", c => c.Int());
            DropColumn("dbo.GoiTaps", "MoTa");
            DropColumn("dbo.GoiTaps", "SoBuoi");
            DropColumn("dbo.GoiTaps", "ThoiHanSuDung");
            DropColumn("dbo.AspNetUsers", "NgaySinh");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "NgaySinh", c => c.DateTime());
            AddColumn("dbo.GoiTaps", "ThoiHanSuDung", c => c.Int(nullable: false));
            AddColumn("dbo.GoiTaps", "SoBuoi", c => c.Int(nullable: false));
            AddColumn("dbo.GoiTaps", "MoTa", c => c.String());
            DropForeignKey("dbo.HuanLuyenViens", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HoiViens", "GoiTapId", "dbo.GoiTaps");
            DropForeignKey("dbo.HoiViens", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.HuanLuyenViens", new[] { "ApplicationUserId" });
            DropIndex("dbo.HoiViens", new[] { "GoiTapId" });
            DropIndex("dbo.HoiViens", new[] { "ApplicationUserId" });
            DropColumn("dbo.AspNetUsers", "HuanLuyenVienId");
            DropColumn("dbo.AspNetUsers", "HoiVienId");
            DropColumn("dbo.AspNetUsers", "VaiTro");
            DropColumn("dbo.GoiTaps", "SoBuoiTapVoiPT");
            DropColumn("dbo.GoiTaps", "MoTaQuyenLoi");
            DropTable("dbo.HuanLuyenViens");
            DropTable("dbo.HoiViens");
        }
    }
}
