using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExceptionHandler;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError($"An error occurred: {ex}");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "text/plain"; 
            await context.Response.WriteAsync(ex.Message);
        }
        catch (IllegalArgumentException ex)
        {
            _logger.LogError($"An error occurred: {ex}");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "text/plain"; 
            await context.Response.WriteAsync(ex.Message);
        }
        // catch (Exception ex)
        // {
        //     _logger.LogError($"An unexpected error occurred: {ex.Message}");
        //     context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //     context.Response.ContentType = "text/plain";
        //     await context.Response.WriteAsync("An unexpected error occurred.");
        // }
    }
 
}