namespace FD.TaskManager.API.Extentions;

public class ProfileMiddleware
{
    private readonly RequestDelegate _next;
    public ProfileMiddleware(RequestDelegate next)
    {
        _next = next ??
            throw new ArgumentNullException(nameof(next));
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Equals("/profile.html", StringComparison.OrdinalIgnoreCase))
        {
            var token = context.Request.Cookies["auth-token"];
            if (string.IsNullOrEmpty(token))
            {
                context.Response.Redirect("/login.html");
                return;
            }
        }
        await _next(context);
    }
}
