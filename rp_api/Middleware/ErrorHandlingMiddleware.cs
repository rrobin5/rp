using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Net;
using System.Text.Json;

namespace rp_api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.NotFound, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (MongoWriteException ex)
            {
                Console.WriteLine($"Error de escritura en MongoDB: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Tiempo de espera excedido: {ex.Message}");
            }
            catch (MongoException ex)
            {
                Console.WriteLine($"Error general de MongoDB: {ex.Message}");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later. " + ex.Message);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                error = new
                {
                    code = context.Response.StatusCode,
                    message
                }
            });

            return context.Response.WriteAsync(result);
        }
    }

}
