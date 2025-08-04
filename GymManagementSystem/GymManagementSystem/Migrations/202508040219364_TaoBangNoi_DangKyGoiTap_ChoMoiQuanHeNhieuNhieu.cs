namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaoBangNoi_DangKyGoiTap_ChoMoiQuanHeNhieuNhieu : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HoiViens", "GoiTapId", "dbo.GoiTaps");
            DropIndex("dbo.HoiViens", new[] { "GoiTapId" });
            CreateTable(
                "dbo.DangKyGoiTaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoiVienId = c.Int(nullable: false),
                        GoiTapId = c.Int(nullable: false),
                        NgayDangKy = c.DateTime(nullable: false),
                        NgayHetHan = c.DateTime(),
                        TrangThai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoiTaps", t => t.GoiTapId, cascadeDelete: true)
                .ForeignKey("dbo.HoiViens", t => t.HoiVienId, cascadeDelete: true)
                .Index(t => t.HoiVienId)
                .Index(t => t.GoiTapId);
            
            DropColumn("dbo.HoiViens", "GoiTapId");
            DropColumn("dbo.HoiViens", "NgayHetHanGoiTap");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoiViens", "NgayHetHanGoiTap", c => c.DateTime());
            AddColumn("dbo.HoiViens", "GoiTapId", c => c.Int());
            DropForeignKey("dbo.DangKyGoiTaps", "HoiVienId", "dbo.HoiViens");
            DropForeignKey("dbo.DangKyGoiTaps", "GoiTapId", "dbo.GoiTaps");
            DropIndex("dbo.DangKyGoiTaps", new[] { "GoiTapId" });
            DropIndex("dbo.DangKyGoiTaps", new[] { "HoiVienId" });
            DropTable("dbo.DangKyGoiTaps");
            CreateIndex("dbo.HoiViens", "GoiTapId");
            AddForeignKey("dbo.HoiViens", "GoiTapId", "dbo.GoiTaps", "Id");
        }
    }
}
