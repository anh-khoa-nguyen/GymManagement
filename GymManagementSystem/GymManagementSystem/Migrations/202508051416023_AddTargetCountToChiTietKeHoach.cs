namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTargetCountToChiTietKeHoach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChiTietKeHoaches", "SoLanMucTieu", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChiTietKeHoaches", "SoLanMucTieu");
        }
    }
}
