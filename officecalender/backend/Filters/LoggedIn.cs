using Microsoft.AspNetCore.Mvc.Filters;
public class LoggedIn : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
    {
        if (!actionContext.HttpContext.Request.Headers.ContainsKey("LoggedIn"))
        {
            actionContext.HttpContext.Response.StatusCode = 401;
            return;
        }
        if (actionContext.HttpContext.Request.Headers["LoggedIn"] != "true")
        {
            actionContext.HttpContext.Response.StatusCode = 401;
            return;
        }
        await next.Invoke();
        return;
    }

}