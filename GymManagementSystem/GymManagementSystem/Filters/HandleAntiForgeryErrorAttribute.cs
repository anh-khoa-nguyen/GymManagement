using System.Web.Mvc;

namespace GymManagementSystem.Filters
{
    public class HandleAntiForgeryErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is HttpAntiForgeryException)
            {
                // Đánh dấu là lỗi đã được xử lý
                filterContext.ExceptionHandled = true;

                // Tạo một kết quả để chuyển hướng người dùng đến trang Login
                var result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" }
                    });

                // Gán kết quả chuyển hướng này cho context
                filterContext.Result = result;
            }
            else
            {
                // Nếu là lỗi khác, cứ để cơ chế mặc định xử lý
                base.OnException(filterContext);
            }
        }
    }
}