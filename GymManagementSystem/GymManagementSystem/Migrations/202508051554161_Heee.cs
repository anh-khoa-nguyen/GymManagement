namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Heee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoiViens", "MaGioiThieu", c => c.String(maxLength: 6));
            CreateIndex("dbo.HoiViens", "MaGioiThieu", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.HoiViens", new[] { "MaGioiThieu" });
            DropColumn("dbo.HoiViens", "MaGioiThieu");
        }
    }
}
