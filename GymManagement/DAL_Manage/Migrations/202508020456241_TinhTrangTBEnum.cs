namespace DAL_Manage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TinhTrangTBEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ThietBi", "Tinh_Trang", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ThietBi", "Tinh_Trang", c => c.String(maxLength: 255));
        }
    }
}
