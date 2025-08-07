namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaoBang_ThongBao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ThongBaos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        NoiDung = c.String(nullable: false, maxLength: 500),
                        URL = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                        DaXem = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThongBaos", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.ThongBaos", new[] { "ApplicationUserId" });
            DropTable("dbo.ThongBaos");
        }
    }
}
