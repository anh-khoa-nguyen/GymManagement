namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePhongIdNullableInThietBi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ThietBis", "PhongId", "dbo.Phongs");
            DropIndex("dbo.ThietBis", new[] { "PhongId" });
            AlterColumn("dbo.ThietBis", "PhongId", c => c.Int());
            CreateIndex("dbo.ThietBis", "PhongId");
            AddForeignKey("dbo.ThietBis", "PhongId", "dbo.Phongs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThietBis", "PhongId", "dbo.Phongs");
            DropIndex("dbo.ThietBis", new[] { "PhongId" });
            AlterColumn("dbo.ThietBis", "PhongId", c => c.Int(nullable: false));
            CreateIndex("dbo.ThietBis", "PhongId");
            AddForeignKey("dbo.ThietBis", "PhongId", "dbo.Phongs", "Id", cascadeDelete: true);
        }
    }
}
