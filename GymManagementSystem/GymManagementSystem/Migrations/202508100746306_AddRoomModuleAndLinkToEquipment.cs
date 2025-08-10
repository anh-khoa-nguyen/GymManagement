namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoomModuleAndLinkToEquipment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Phongs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenPhong = c.String(nullable: false, maxLength: 100),
                        MaPhong = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ThietBis", "PhongId", c => c.Int(nullable: true));
            CreateIndex("dbo.ThietBis", "PhongId");
            AddForeignKey("dbo.ThietBis", "PhongId", "dbo.Phongs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThietBis", "PhongId", "dbo.Phongs");
            DropIndex("dbo.ThietBis", new[] { "PhongId" });
            DropColumn("dbo.ThietBis", "PhongId");
            DropTable("dbo.Phongs");
        }
    }
}
