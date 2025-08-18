namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChieuCaoLoiKhuyentoChiSoSK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChiSoSucKhoes", "ChieuCao", c => c.Double());
            AddColumn("dbo.ChiSoSucKhoes", "LoiKhuyenAI", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChiSoSucKhoes", "ChieuCao");
            DropColumn("dbo.ChiSoSucKhoes", "LoiKhuyenAI");
        }
    }
}
