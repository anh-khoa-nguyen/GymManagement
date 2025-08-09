// Trong file Attributes/AjaxAuthorizeAttribute.cs
using System.Web.Mvc;

namespace GymManagementSystem.Attributes
{
    public class AjaxAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // Nếu là AJAX, trả về lỗi 401 Unauthorized thay vì redirect
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}