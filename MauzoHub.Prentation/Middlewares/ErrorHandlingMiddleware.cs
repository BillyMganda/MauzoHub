using MauzoHub.Application.CustomExceptions;
using System.Net;

namespace MauzoHub.Prentation.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";

            if (ex is BadRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = "Bad request exception occured";
            }
            else if (ex is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = "Not found exception occured";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsJsonAsync(new { error = message });
        }
    }
}
