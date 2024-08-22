using System.Net;
using System.Text.Json;
using BusinessObjects.Entities;
using LoggerService;
using Microsoft.Extensions.Primitives;
using Tools;

namespace eStore.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CustomException.InvalidDataException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch (CustomException.DataNotFoundException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
    {
        logger.LogError($"Something went wrong: {ex}");
        var result = JsonSerializer.Serialize(new { error = ex.Message });
        context.Response.ContentType = "application/json";
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(result);
    }
}