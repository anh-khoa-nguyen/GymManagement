namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyToMany_HangHoiVien_KhuyenMai : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HangCoKhuyenMais",
                c => new
                    {
                        HangHoiVienId = c.Int(nullable: false),
                        KhuyenMaiId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HangHoiVienId, t.KhuyenMaiId })
                .ForeignKey("dbo.HangHoiViens", t => t.HangHoiVienId, cascadeDelete: true)
                .ForeignKey("dbo.KhuyenMais", t => t.KhuyenMaiId, cascadeDelete: true)
                .Index(t => t.HangHoiVienId)
                .Index(t => t.KhuyenMaiId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HangCoKhuyenMais", "KhuyenMaiId", "dbo.KhuyenMais");
            DropForeignKey("dbo.HangCoKhuyenMais", "HangHoiVienId", "dbo.HangHoiViens");
            DropIndex("dbo.HangCoKhuyenMais", new[] { "KhuyenMaiId" });
            DropIndex("dbo.HangCoKhuyenMais", new[] { "HangHoiVienId" });
            DropTable("dbo.HangCoKhuyenMais");
        }
    }
}
