namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogThietBiChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LichSuThietBis", "TrangThaiTruoc", c => c.String());
            AddColumn("dbo.LichSuThietBis", "TrangThaiSau", c => c.String());
            DropColumn("dbo.LichSuThietBis", "MoTaThayDoi");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LichSuThietBis", "MoTaThayDoi", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.LichSuThietBis", "TrangThaiSau");
            DropColumn("dbo.LichSuThietBis", "TrangThaiTruoc");
        }
    }
}
