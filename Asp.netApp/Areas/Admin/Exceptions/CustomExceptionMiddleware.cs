using Microsoft.AspNetCore.Mvc;

namespace Asp.netApp.Areas.Admin.Exceptions
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.Clear();

            if (exception is NotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.Redirect("/Admin/Error/NotFound");
            }
            else if (exception is ForbiddenException)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.Redirect("/Admin/Error/Forbidden");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.Redirect("/Admin/Error/InternalServerError");
            }

            return Task.CompletedTask;
        }
    }
}
