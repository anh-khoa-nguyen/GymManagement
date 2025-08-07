namespace GymManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedJumpingJacksExercise : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            INSERT INTO dbo.BaiTaps (TenBaiTap, MoTa, NhomCoChinh, NhomCoPhu, DungCu, MucDo, ImageUrl, VideoUrl) 
            VALUES (N'Nhảy Dây Tại Chỗ (Jumping Jacks)', N'Bài tập cardio đơn giản giúp làm nóng toàn thân, tăng nhịp tim và cải thiện sức bền.', N'Toàn thân', N'Bắp chân, Vai', N'Không cần dụng cụ', N'Cơ bản', NULL, NULL)
        ");

            // Thêm các bước thực hiện cho bài tập vừa tạo (giả sử Id của nó là 1)
            Sql(@"
            INSERT INTO dbo.BuocThucHiens (BaiTapId, ThuTuBuoc, NoiDung) VALUES
            (1, 1, N'Bắt đầu ở tư thế đứng thẳng, hai chân khép, hai tay duỗi thẳng thoải mái hai bên hông.'),
            (1, 2, N'Thực hiện động tác bật nhảy, đồng thời dang rộng hai chân sang hai bên và vung hai tay lên cao qua đầu, gần chạm vào nhau.'),
            (1, 3, N'Bật nhảy một lần nữa để trở về vị trí ban đầu một cách nhẹ nhàng.'),
            (1, 4, N'Lặp lại động tác liên tục và nhịp nhàng.')
        ");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM dbo.BuocThucHiens WHERE BaiTapId = (SELECT Id FROM dbo.BaiTaps WHERE TenBaiTap = N'Nhảy Dây Tại Chỗ (Jumping Jacks)')");
            Sql("DELETE FROM dbo.BaiTaps WHERE TenBaiTap = N'Nhảy Dây Tại Chỗ (Jumping Jacks)')");
        }
    }
}
