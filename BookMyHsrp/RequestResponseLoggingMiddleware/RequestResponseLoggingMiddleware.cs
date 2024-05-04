using System.Diagnostics;
using static BookMyHsrp.Libraries.RequestResponseLoggingMiddleware.Model.RequestResponseLoggingMiddlewareModel;

namespace BookMyHsrp.RequestResponseLoggingMiddleware
{
    public interface ILoggingService
    {
        void Log(RequestResponseLog newData);
    }

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsApiV1Controller(context))
            {
                using var scope = _serviceProvider.CreateScope();
                var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

                var stopwatch = Stopwatch.StartNew();

                var request = await FormatRequest(context.Request);
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);
                    stopwatch.Stop();
                    var responseText = await FormatResponse(context.Response);
                    var newData = new RequestResponseLog
                    {
                        Timestamp = DateTime.UtcNow,
                        Request = request,
                        Response = responseText,
                        UserAgent = context.Request.Headers["User-Agent"],
                        IPAddress = context.Connection.RemoteIpAddress.ToString(),
                        RequestUrl = context.Request.Path,
                        TimeTakenMs = stopwatch.ElapsedMilliseconds,
                        // Add other fields as needed
                    };

                    loggingService.Log(newData);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private bool IsApiV1Controller(HttpContext context)
        {
            // Check if the route starts with "/api/v1"
            var route = context.Request.Path.StartsWithSegments("/api");
            return route;
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0;

            return body;
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }


   
}
