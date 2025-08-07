namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Microsoft.AspNet.Identity;

    public partial class SeedInitialRolesAndAdmin : DbMigration
    {
        public override void Up()
        {
            //// --- TẠO CÁC VAI TRÒ CẦN THIẾT ---
            //Sql("INSERT INTO dbo.AspNetRoles (Id, Name) VALUES (NEWID(), 'QuanLy')");
            //Sql("INSERT INTO dbo.AspNetRoles (Id, Name) VALUES (NEWID(), 'PT')");
            //Sql("INSERT INTO dbo.AspNetRoles (Id, Name) VALUES (NEWID(), 'HoiVien')");

            //// --- TẠO NGƯỜI DÙNG QUẢN LÝ ---
            //var adminUserId = Guid.NewGuid().ToString();
            //var adminUserName = "quanly";
            //var adminEmail = "quanly@gym.com";
            //var hoTen = "Quản Lý Viên";
            //var vaiTro = "QuanLy";

            //var passwordHasher = new PasswordHasher();
            //var hashedPassword = passwordHasher.HashPassword("Abc@123"); // Đặt mật khẩu của bạn ở đây

            //Sql($@"INSERT INTO dbo.AspNetUsers (Id, HoTen, VaiTro, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName)
            //       VALUES ('{adminUserId}', N'{hoTen}', N'{vaiTro}', '{adminEmail}', 1, '{hashedPassword}', NEWID(), NULL, 0, 0, '2000-01-01', 1, 0, '{adminUserName}')");

            //// --- GÁN VAI TRÒ "QuanLy" CHO NGƯỜI DÙNG VỪA TẠO ---
            //Sql($@"INSERT INTO dbo.AspNetUserRoles (UserId, RoleId) 
            //       SELECT '{adminUserId}', Id FROM dbo.AspNetRoles WHERE Name = 'QuanLy'");
        }
        
        public override void Down()
        {
            //Sql("DELETE FROM dbo.AspNetUserRoles WHERE UserId IN (SELECT Id FROM dbo.AspNetUsers WHERE UserName = 'quanly')");
            //Sql("DELETE FROM dbo.AspNetUsers WHERE UserName = 'quanly'");
            //Sql("DELETE FROM dbo.AspNetRoles WHERE Name IN ('QuanLy', 'PT', 'HoiVien')");
        }
    }
}
