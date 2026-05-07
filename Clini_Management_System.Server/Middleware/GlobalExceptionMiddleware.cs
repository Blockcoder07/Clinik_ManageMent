using System.Net.Mime;
using System.Text.Json;
using Clini_Management_System.Server.Common;

namespace Clini_Management_System.Server.Middleware;

public sealed class GlobalExceptionMiddleware
{
    #region Fields

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    #endregion

    #region Constructor

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    #endregion

    #region Public Methods

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException ex)
        {
            _logger.LogWarning("API exception {StatusCode} on {Method} {Path}: {Message}",
                ex.StatusCode, context.Request.Method, context.Request.Path, ex.Message);
            await WriteAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                context.Request.Method, context.Request.Path);
            await WriteAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    #endregion

    #region Private Methods

    private static Task WriteAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var payload = ApiResponse<object>.Fail(message);
        return context.Response.WriteAsync(JsonSerializer.Serialize(payload, JsonOptions));
    }

    #endregion
}
