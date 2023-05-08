namespace PeopleFinder.Api.Middlewares;

public class AuthTokenSetterMiddleware
{
    private readonly RequestDelegate _next;
    
    public AuthTokenSetterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string? token = context.Request.Cookies["accessToken"];

        if (!string.IsNullOrEmpty(token))
            context.Request.Headers.TryAdd("Authorization", "Bearer " + token);
        
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Xss-Protection", "1");
        context.Response.Headers.Add("X-Frame-Options", "DENY");

        await _next(context);
    }
}