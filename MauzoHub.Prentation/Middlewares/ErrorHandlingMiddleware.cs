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
            var message = "Operation not successful, an unexpected error occurred.";

            if (ex is BadRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = "Operation not successful, bad request";
            }
            else if (ex is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = "Operation not successful, not found";
            }
            else if (ex is UnauthorizedException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = "Operation not successful, unauthorized";
            }
            else if (ex is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = "Operation not successful, bad request";
            }
            else if (ex is ForbiddenException)
            {
                statusCode = HttpStatusCode.Forbidden;
                message = "Operation not successful, forbidden";
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = "Operation not successful, internal server error";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsJsonAsync(new { error = message });
        }
    }
}
