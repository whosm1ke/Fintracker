using Newtonsoft.Json;

namespace Fintracker.API.Middleware;

public class UnauthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            context.Response.ContentType = "application/json";
            
            string result = JsonConvert.SerializeObject(new ErrorDeatils
            {
                ErrorMessage = "Notnotnot",
                ErrorType = "Failure"
            });
            
            await context.Response.WriteAsync(result);
        }
    }
}

public static class UnauthorizedMiddlewareExtensions
{
    public static IApplicationBuilder UseUnauthorizedMiddleware(this IApplicationBuilder app)
    {
        
        return app.UseMiddleware<UnauthorizedMiddleware>();
    }
}