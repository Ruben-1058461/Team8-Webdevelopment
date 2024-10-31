using Microsoft.AspNetCore.Mvc.Filters;
public class AdminOnly : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
    {
        if (!actionContext.HttpContext.Request.Headers.ContainsKey("AdminOnly"))
        {
            actionContext.HttpContext.Response.StatusCode = 401;
            return;
        }
        if (actionContext.HttpContext.Request.Headers["AdminOnly"] != "true")
        {
            actionContext.HttpContext.Response.StatusCode = 401;
            return;
        }
        await next.Invoke();
        return;
    }

}