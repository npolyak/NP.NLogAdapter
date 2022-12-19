using NLog;
using NP.DependencyInjection.Attributes;
using NP.Utilities;

namespace NP.NLogAdapter
{
    [RegisterType(typeof(ILog))]
    public class NLogWrapper : ILog
    {
        public static NLogWrapper Instance { get; } = new NLogWrapper();

        private ILogger _logger;

        public NLogWrapper()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Log(LogKind logKind, string component, string msg)
        {
            LogEventInfo logEventInfo = new LogEventInfo(logKind.ToLogLevel(), "DefaultLogger", $"{msg}");

            logEventInfo.Properties.Add("Component", component);

            _logger.Log(logEventInfo);
        }

        public static void SetLog()
        {
            NP.Utilities.Logger.SetLog(Instance);
        }
    }
}
