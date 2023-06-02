using System.Net;
using System.Text.Json;
using AudioPlayerWebAPI.UseCase.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace AudioPlayerWebAPI.Middleware;

public class CustomExceptionHandler
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        string? result = null;
        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = validationException.Errors.First().ErrorMessage;
                break;
            case NotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case NotAllowedFileException:
                code = HttpStatusCode.BadRequest;
                break;
            case EmailAlreadyInUseException:
                code = HttpStatusCode.BadRequest;
                break;
            case RefreshTokenException:
                code = HttpStatusCode.BadRequest;
                break;
            case InvalidLoginException:
                code = HttpStatusCode.BadRequest;
                break;
            default:
                code = HttpStatusCode.BadRequest;
                break;
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var response = JsonSerializer.Serialize(new
        {
            error = result ?? exception.Message,
            code
        });

        return context.Response.WriteAsync(response);
    }
}