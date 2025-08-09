namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEquipmentHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LichSuThietBis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThietBiId = c.Int(nullable: false),
                        NguoiThucHienId = c.String(nullable: false, maxLength: 128),
                        HanhDong = c.Int(nullable: false),
                        NgayThucHien = c.DateTime(nullable: false),
                        MoTaThayDoi = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.NguoiThucHienId, cascadeDelete: true)
                .Index(t => t.NguoiThucHienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LichSuThietBis", "NguoiThucHienId", "dbo.AspNetUsers");
            DropIndex("dbo.LichSuThietBis", new[] { "NguoiThucHienId" });
            DropTable("dbo.LichSuThietBis");
        }
    }
}
