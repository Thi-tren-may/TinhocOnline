using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TinhocOnline.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class BaseTeacherController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "teacher")
            {
                context.Result = RedirectToAction("Login", "Auth", new { area = "" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
