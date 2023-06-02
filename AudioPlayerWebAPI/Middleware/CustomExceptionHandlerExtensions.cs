namespace AudioPlayerWebAPI.Middleware;

public static class CustomExceptionHandlerExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandler>();
    }

}