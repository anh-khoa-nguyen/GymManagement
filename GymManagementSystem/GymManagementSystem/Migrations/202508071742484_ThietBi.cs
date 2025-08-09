namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThietBi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ThietBis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenThietBi = c.String(nullable: false, maxLength: 100),
                        MoTa = c.String(),
                        NgayMua = c.DateTime(),
                        TinhTrang = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ThietBis");
        }
    }
}
