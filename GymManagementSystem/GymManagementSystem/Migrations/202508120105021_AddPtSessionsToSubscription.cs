namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPtSessionsToSubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DangKyGoiTaps", "SoBuoiTapVoiPT", c => c.Int(nullable: false));
            AddColumn("dbo.DangKyGoiTaps", "SoBuoiPTDaSuDung", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DangKyGoiTaps", "SoBuoiPTDaSuDung");
            DropColumn("dbo.DangKyGoiTaps", "SoBuoiTapVoiPT");
        }
    }
}
