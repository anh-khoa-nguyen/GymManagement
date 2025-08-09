namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageUrls : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AvatarUrl", c => c.String());
            AddColumn("dbo.GoiTaps", "ImageUrl", c => c.String());
            AddColumn("dbo.KeHoaches", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KeHoaches", "ImageUrl");
            DropColumn("dbo.GoiTaps", "ImageUrl");
            DropColumn("dbo.AspNetUsers", "AvatarUrl");
        }
    }
}
