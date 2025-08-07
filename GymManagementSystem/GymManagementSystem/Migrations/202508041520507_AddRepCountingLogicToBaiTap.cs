namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRepCountingLogicToBaiTap : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaiTaps", "RepCountingLogic", c => c.String(maxLength: 100));
            Sql("UPDATE dbo.BaiTaps SET RepCountingLogic = 'jumpingJacksCounter' WHERE TenBaiTap = N'Nhảy Dây Tại Chỗ (Jumping Jacks)'");
        }

        public override void Down()
        {
            DropColumn("dbo.BaiTaps", "RepCountingLogic");
        }
    }
}
