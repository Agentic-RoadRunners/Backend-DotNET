
using System.Net;
using System.Text.Json;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.WebApi.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            NotFoundException ex => new { StatusCode = (int)HttpStatusCode.NotFound, Body = ApiResponse<string>.Fail(ex.Message) },
            ForbiddenException ex => new { StatusCode = (int)HttpStatusCode.Forbidden, Body = ApiResponse<string>.Fail(ex.Message) },
            BadRequestException ex => new { StatusCode = (int)HttpStatusCode.BadRequest, Body = ApiResponse<string>.Fail(ex.Message, ex.Errors) },
            UnauthorizedException ex => new { StatusCode = (int)HttpStatusCode.Unauthorized, Body = ApiResponse<string>.Fail(ex.Message) },
            _ => new { StatusCode = (int)HttpStatusCode.InternalServerError, Body = ApiResponse<string>.Fail($"Error: {exception.Message} | Inner: {exception.InnerException?.Message}") }
        };

        context.Response.StatusCode = response.StatusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response.Body, options));
    }
}