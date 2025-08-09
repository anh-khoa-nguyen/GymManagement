namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentAttr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhuyenMais", "SoTienGiamToiDa", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.KhuyenMais", "SoNgayHieuLuc", c => c.Int(nullable: false));
            DropColumn("dbo.KhuyenMais", "SoTienGiamGia");
            DropColumn("dbo.KhuyenMais", "NgayBatDau");
            DropColumn("dbo.KhuyenMais", "NgayKetThuc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KhuyenMais", "NgayKetThuc", c => c.DateTime(nullable: false));
            AddColumn("dbo.KhuyenMais", "NgayBatDau", c => c.DateTime(nullable: false));
            AddColumn("dbo.KhuyenMais", "SoTienGiamGia", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.KhuyenMais", "SoNgayHieuLuc");
            DropColumn("dbo.KhuyenMais", "SoTienGiamToiDa");
        }
    }
}
