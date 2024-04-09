using Fintracker.Application.Exceptions;
using Fintracker.Application.Responses.API_Responses;
using Serilog;

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
                Log.Error("{ErrorName} [{TraceId}] [{When}]:\n{Reason}\n{@Details} [{StatusCode}]",
                    nameof(BadRequestException), badRequestResponse.TraceId, badRequestResponse.When,
                    badRequestResponse.Reason, bad.Errors, badRequestResponse.StatusCode);
                await context.Response.WriteAsJsonAsync(badRequestResponse);
                break;
            case NotFoundException notFound:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                var notFoundResponse = new NotFoundResponse
                {
                    TraceId = Guid.NewGuid(),
                    When = DateTime.UtcNow,
                    Reason = "Not Found",
                    Details = notFound.Errors,
                    Type = notFound.Type,
                    StatusCode = StatusCodes.Status404NotFound
                };
                Log.Error("{ErrorName} [{TraceId}] [{When}]:\n{Reason}\n{@Details} [{StatusCode}]",
                    nameof(NotFoundException), notFoundResponse.TraceId, notFoundResponse.When,
                    notFoundResponse.Reason, notFound.Errors, notFoundResponse.StatusCode);
                await context.Response.WriteAsJsonAsync(notFoundResponse);
                break;
            case LoginException log:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var logResponse = new UnauthorizedResponse
                {
                    TraceId = Guid.NewGuid(),
                    When = DateTime.UtcNow,
                    Reason = "Unauthorized",
                    Details = log.Errors,
                    StatusCode = StatusCodes.Status401Unauthorized,
                };
                Log.Error("{ErrorName} [{TraceId}] [{When}]:\n{Reason}\n{@Details} [{StatusCode}]",
                    nameof(LoginException), logResponse.TraceId, logResponse.When,
                    logResponse.Reason, log.Errors, logResponse.StatusCode);
                await context.Response.WriteAsJsonAsync(logResponse);
                break;
            case RegisterAccountException reg:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                var regResponse = new UnauthorizedResponse
                {
                    TraceId = Guid.NewGuid(),
                    When = DateTime.UtcNow,
                    Reason = "Unauthorized",
                    Details = reg.Errors,
                    StatusCode = StatusCodes.Status409Conflict
                };
                Log.Error("{ErrorName} [{TraceId}] [{When}]:\n{Reason}\n{@Details} [{StatusCode}]",
                    nameof(RegisterAccountException), regResponse.TraceId, regResponse.When,
                    regResponse.Reason, reg.Errors, regResponse.StatusCode);
                await context.Response.WriteAsJsonAsync(regResponse);
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var response = new BaseResponse
                {
                    When = DateTime.UtcNow,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Reason = "Internal Server Error",
                    Message = exception.Message,
                    TraceId = Guid.NewGuid()
                };
                Log.Error("{ErrorName} [{TraceId}] [{When}]:\n{Reason}\n{Message}\n[{StatusCode}]",
                    "InternalException", response.TraceId, response.When.ToShortTimeString(),
                    response.Reason, exception.Message, response.StatusCode);
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