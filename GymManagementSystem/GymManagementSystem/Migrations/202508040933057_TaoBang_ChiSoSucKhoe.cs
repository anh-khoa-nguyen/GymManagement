namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaoBang_ChiSoSucKhoe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiSoSucKhoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoiVienId = c.Int(nullable: false),
                        NgayCapNhat = c.DateTime(nullable: false),
                        CanNang = c.Double(nullable: false),
                        TyLeMo = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HoiViens", t => t.HoiVienId, cascadeDelete: true)
                .Index(t => t.HoiVienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChiSoSucKhoes", "HoiVienId", "dbo.HoiViens");
            DropIndex("dbo.ChiSoSucKhoes", new[] { "HoiVienId" });
            DropTable("dbo.ChiSoSucKhoes");
        }
    }
}
