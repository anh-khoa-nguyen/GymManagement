namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DacQuyen : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HangCoDacQuyens",
                c => new
                    {
                        HangHoiVienId = c.Int(nullable: false),
                        DacQuyenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HangHoiVienId, t.DacQuyenId })
                .ForeignKey("dbo.DacQuyens", t => t.DacQuyenId, cascadeDelete: true)
                .ForeignKey("dbo.HangHoiViens", t => t.HangHoiVienId, cascadeDelete: true)
                .Index(t => t.HangHoiVienId)
                .Index(t => t.DacQuyenId);
            
            CreateTable(
                "dbo.DacQuyens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenDacQuyen = c.String(nullable: false, maxLength: 100),
                        MoTa = c.String(),
                        IconClass = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.HangHoiViens", "DacQuyen");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HangHoiViens", "DacQuyen", c => c.String());
            DropForeignKey("dbo.HangCoDacQuyens", "HangHoiVienId", "dbo.HangHoiViens");
            DropForeignKey("dbo.HangCoDacQuyens", "DacQuyenId", "dbo.DacQuyens");
            DropIndex("dbo.HangCoDacQuyens", new[] { "DacQuyenId" });
            DropIndex("dbo.HangCoDacQuyens", new[] { "HangHoiVienId" });
            DropTable("dbo.DacQuyens");
            DropTable("dbo.HangCoDacQuyens");
        }
    }
}
