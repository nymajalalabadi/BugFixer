using Microsoft.AspNetCore.Mvc.Filters;

namespace BugFixer.Web.ActionFilters
{
    public class RedirectHomeIfLoggedInActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.Redirect("/");
            }
        }
    }

}
