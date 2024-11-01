using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class LoggedInAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var path = context.HttpContext.Request.Path.Value;
        if (path == "/api/auth/login" || path == "/api/auth/logout")
        {
            return;
        }

        var userEmail = context.HttpContext.Session.GetString("UserEmail");

        // Check if the user is logged in
        if (string.IsNullOrEmpty(userEmail))
        {
            // User is not logged in; return 403 Forbidden
            context.Result = new ForbidResult("User is not logged in");
        }

        base.OnActionExecuting(context);
    }
}
