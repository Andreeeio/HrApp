using HrApp.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace HrApp.MVC.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch(UnauthorizedException ex)
        {
            _logger.LogWarning("Unauthorized access attempt: " + ex.Message);

            context.Response.Redirect("/User/Login");

            context.Response.StatusCode = (int)HttpStatusCode.Found;

            return;
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "Internal Server Error from the custom middleware.",
            Detailed = exception.Message
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
