using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static BookMyHsrp.Libraries.ExceptionHandling.Model.ExceptionHandling;

namespace BookMyHsrp.ExceptionHandling
{
    public class ExceptionMiddleWare : IExceptionHandler
    {
        private readonly ILogger<ExceptionMiddleWare> _logger;

        public ExceptionMiddleWare(ILogger<ExceptionMiddleWare> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unexpected error occurred");
            ExceptionLog value = null;
            if (exception.InnerException is ValidationException)
            {
                value = new ExceptionLog
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().Name,
                    Title = exception.InnerException.Message,
                    Detail = "Validation failed",
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    StatusText = "Validation Failed",
                    IsError = true,
                    Message = exception.InnerException.Message,
                    StackTrace = exception.StackTrace
                };
            }
            else
            {
                value = new ExceptionLog
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    StatusText = "Internal Server Error",
                    IsError = true,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace
                };
            }
            await httpContext.Response.WriteAsJsonAsync(value);
            return true;
        }
    }

}

