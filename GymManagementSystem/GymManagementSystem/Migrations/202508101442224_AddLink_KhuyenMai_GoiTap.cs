namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLink_KhuyenMai_GoiTap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KhuyenMaiCuaGois",
                c => new
                    {
                        KhuyenMaiId = c.Int(nullable: false),
                        GoiTapId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.KhuyenMaiId, t.GoiTapId })
                .ForeignKey("dbo.GoiTaps", t => t.GoiTapId, cascadeDelete: true)
                .ForeignKey("dbo.KhuyenMais", t => t.KhuyenMaiId, cascadeDelete: true)
                .Index(t => t.KhuyenMaiId)
                .Index(t => t.GoiTapId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KhuyenMaiCuaGois", "KhuyenMaiId", "dbo.KhuyenMais");
            DropForeignKey("dbo.KhuyenMaiCuaGois", "GoiTapId", "dbo.GoiTaps");
            DropIndex("dbo.KhuyenMaiCuaGois", new[] { "GoiTapId" });
            DropIndex("dbo.KhuyenMaiCuaGois", new[] { "KhuyenMaiId" });
            DropTable("dbo.KhuyenMaiCuaGois");
        }
    }
}
