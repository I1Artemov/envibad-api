using EnviBad.API.Common.Log;
using NLog;

namespace EnviBad.API.Infrastructure.Log
{
    public class AppLoggerImpl : IAppLogger
    {
        private readonly ILogger _logger;

        public AppLoggerImpl(ILogger logger)
        {
            _logger = logger;
        }

        public void Error(Exception ex)
        {
            _logger.Error(ex);
        }

        public void Error(Exception ex, string message)
        {
            _logger.Error(ex, message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }
    }
}
