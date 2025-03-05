using ApiTask.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ApiTask
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Uma exceção ocorreu.");
      
                httpContext.Response.ContentType = "application/json";
                switch (ex)
                {
                    case UnauthorizedAccessException _:
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        break;
                    case ForbiddenAccessException _:
                        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                        break;
                    case ArgumentException _:
                    case ValidationException _:
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case Exception _:
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                var response = new
                {
                    message = ex.Message
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }        
        }
    }
}
