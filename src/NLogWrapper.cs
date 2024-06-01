using NLog;
using NP.DependencyInjection.Attributes;
using NP.Utilities;

namespace NP.NLogAdapter
{
    [RegisterType(typeof(ILog))]
    public class NLogWrapper : ILog
    {
        public static NLogWrapper Instance { get; } = new NLogWrapper();

        private readonly ILogger _logger;

        public NLogWrapper()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Log(LogKind logKind, string component, string msg)
        {
            var logLevel = logKind.ToLogLevel();
            if (!_logger.IsEnabled(logLevel))
                return;  // Skip logging when not active

            var logEventInfo = new LogEventInfo(logLevel, _logger.Name, msg);

            if (!string.IsNullOrEmpty(component))
            {
                // Consider making it Logger-Name-Suffix, so one can change NLog MinLevel for specific component
                logEventInfo.Properties.Add("Component", component);
            }

            _logger.Log(typeof(ILog), logEventInfo);  // Support NLog ${CallSite}
        }

        public static void SetLog()
        {
            NP.Utilities.Logger.SetLog(Instance);
        }
    }
}
