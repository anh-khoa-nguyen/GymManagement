namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Them_DanhGia_Vao_LichTap : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LichTaps", "DanhGiaSao", c => c.Int());
            AddColumn("dbo.LichTaps", "PhanHoi", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LichTaps", "PhanHoi");
            DropColumn("dbo.LichTaps", "DanhGiaSao");
        }
    }
}
