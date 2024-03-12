using Fintracker.Application.Exceptions;
using Fintracker.Application.Responses.API_Responses;

namespace Fintracker.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case BadRequestException bad:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var badRequestResponse = new BaseResponse
                {
                    When = DateTime.UtcNow,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Reason = "Bad Request",
                    Details = bad.Errors,
                    TraceId = Guid.NewGuid()
                };
                await context.Response.WriteAsJsonAsync(badRequestResponse);
                break;
            case NotFoundException notFound:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                var notFountResponse = new NotFoundResponse
                {
                    TraceId = Guid.NewGuid(),
                    When = DateTime.UtcNow,
                    Reason = "Not Found",
                    Details = notFound.Errors,
                    Type = notFound.Type,
                    StatusCode = StatusCodes.Status404NotFound
                };
                await context.Response.WriteAsJsonAsync(notFountResponse);
                break;
            case LoginException log:
                var logResponse = new UnauthorizedResponse
                {
                    TraceId = Guid.NewGuid(),
                    When = DateTime.UtcNow,
                    Reason = "Unauthorized",
                    Details = log.Errors,
                    StatusCode = StatusCodes.Status401Unauthorized,
                };
                await context.Response.WriteAsJsonAsync(logResponse);
                break;
            case RegisterAccountException reg:
                var regResponse = new UnauthorizedResponse
                {
                    TraceId = Guid.NewGuid(),
                    When = DateTime.UtcNow,
                    Reason = "Unauthorized",
                    Details = reg.Errors,
                    StatusCode = StatusCodes.Status409Conflict
                };
                await context.Response.WriteAsJsonAsync(regResponse);
                break;
            default:
                var response = new BaseResponse
                {
                    When = DateTime.UtcNow,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Reason = "Internal Server Error",
                    Message = exception.Message,
                    TraceId = Guid.NewGuid()
                };
                await context.Response.WriteAsJsonAsync(response);
                break;
        }
    }
}


public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}