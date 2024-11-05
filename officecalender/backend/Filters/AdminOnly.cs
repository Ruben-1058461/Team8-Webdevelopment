using Microsoft.AspNetCore.Mvc.Filters;
public class AdminOnly : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
    {
        // Check if the user is logged in as an admin
        var isAdmin = actionContext.HttpContext.Session.GetString("is_admin");

        if (string.IsNullOrEmpty(isAdmin) || isAdmin != "true")
        {
            actionContext.HttpContext.Response.StatusCode = 403; // Forbidden
            await actionContext.HttpContext.Response.WriteAsync("Admin access only.");
            return;
        }

        await next.Invoke();
    }
}
