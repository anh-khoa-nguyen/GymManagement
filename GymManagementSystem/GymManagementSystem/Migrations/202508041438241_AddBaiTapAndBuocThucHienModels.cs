namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBaiTapAndBuocThucHienModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaiTaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenBaiTap = c.String(nullable: false, maxLength: 150),
                        MoTa = c.String(),
                        NhomCoChinh = c.String(nullable: false, maxLength: 100),
                        NhomCoPhu = c.String(maxLength: 100),
                        DungCu = c.String(maxLength: 200),
                        MucDo = c.String(),
                        ImageUrl = c.String(),
                        VideoUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuocThucHiens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BaiTapId = c.Int(nullable: false),
                        ThuTuBuoc = c.Int(nullable: false),
                        NoiDung = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaiTaps", t => t.BaiTapId, cascadeDelete: true)
                .Index(t => t.BaiTapId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BuocThucHiens", "BaiTapId", "dbo.BaiTaps");
            DropIndex("dbo.BuocThucHiens", new[] { "BaiTapId" });
            DropTable("dbo.BuocThucHiens");
            DropTable("dbo.BaiTaps");
        }
    }
}
