
using Newtonsoft.Json;
using static BookMyHsrp.Libraries.RequestResponseLoggingMiddleware.Model.RequestResponseLoggingMiddlewareModel;

namespace BookMyHsrp.RequestResponseLoggingMiddleware
{
    public class LoggingService: ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void Log(RequestResponseLog data)
        {
            _logger.LogInformation("Request-Response Log: {SerializeObject}", JsonConvert.SerializeObject(data));
        }
    }
}
