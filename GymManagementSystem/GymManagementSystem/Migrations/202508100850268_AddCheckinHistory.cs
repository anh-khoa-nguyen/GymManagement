namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckinHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LichSuCheckins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        VaiTro = c.String(nullable: false, maxLength: 50),
                        ThoiGianCheckin = c.DateTime(nullable: false),
                        ThanhCong = c.Boolean(nullable: false),
                        GhiChu = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LichSuCheckins", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.LichSuCheckins", new[] { "ApplicationUserId" });
            DropTable("dbo.LichSuCheckins");
        }
    }
}
