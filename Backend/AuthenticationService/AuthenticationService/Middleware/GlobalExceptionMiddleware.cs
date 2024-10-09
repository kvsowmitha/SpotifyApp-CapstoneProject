using System.Net;
using System.Text.Json;
using AuthenticationService.Exceptions;

namespace AuthenticationService.Middleware
{
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case UserNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
                    break;
                case InvalidCredentialsException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
                    break;
                case PasswordMismatchException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
                    break;
                default:
                    _logger.LogError(exception, "An unexpected error occurred.");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "An unexpected error occurred." }));
                    break;
            }
        }
    }
}
