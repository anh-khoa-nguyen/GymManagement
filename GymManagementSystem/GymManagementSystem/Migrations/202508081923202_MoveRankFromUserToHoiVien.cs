namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveRankFromUserToHoiVien : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "HangHoiVienId", "dbo.HangHoiViens");
            DropIndex("dbo.AspNetUsers", new[] { "HangHoiVienId" });
            AddColumn("dbo.HoiViens", "HangHoiVienId", c => c.Int());
            CreateIndex("dbo.HoiViens", "HangHoiVienId");
            AddForeignKey("dbo.HoiViens", "HangHoiVienId", "dbo.HangHoiViens", "Id");
            DropColumn("dbo.AspNetUsers", "HangHoiVienId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "HangHoiVienId", c => c.Int());
            DropForeignKey("dbo.HoiViens", "HangHoiVienId", "dbo.HangHoiViens");
            DropIndex("dbo.HoiViens", new[] { "HangHoiVienId" });
            DropColumn("dbo.HoiViens", "HangHoiVienId");
            CreateIndex("dbo.AspNetUsers", "HangHoiVienId");
            AddForeignKey("dbo.AspNetUsers", "HangHoiVienId", "dbo.HangHoiViens", "Id");
        }
    }
}
