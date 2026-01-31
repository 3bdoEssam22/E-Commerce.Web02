using DomainLayer.Exceptions;
using Shared.ErrorsModels;
using System.Threading.Tasks;

namespace E_Commerce.Web.CustomMiddlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)

        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);

                await HandleNotFoundEndpointAsunc(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something Went wrong");

                await HandleExciptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExciptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ErrorToReturn() { StatusCode = httpContext.Response.StatusCode, ErrorMessage = ex.Message };

            await httpContext.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandleNotFoundEndpointAsunc(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"Endpoint {httpContext.Request.Path} is not found"
                };

                await httpContext.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}
