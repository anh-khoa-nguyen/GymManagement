namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMomo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "MomoOrderId", c => c.String());
            AddColumn("dbo.HoaDons", "PayUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "PayUrl");
            DropColumn("dbo.HoaDons", "MomoOrderId");
        }
    }
}
