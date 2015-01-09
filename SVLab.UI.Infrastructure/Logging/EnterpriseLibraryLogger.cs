using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.Prism.Logging;
using System.Diagnostics;
using System.IO;

namespace SVLab.UI.Infrastructure.Logging
{
    public class EnterpriseLibraryLogger : ILoggerFacade
    {
        //private readonly LogWriter writer = null;

        public EnterpriseLibraryLogger()
        {
            Logger.SetLogWriter(new LogWriter(LoggerConfig()));
        }

        #region ILoggerFacade Members

        public void Log(string message, Category category, Priority priority)
        {
            Logger.Write(message, category.ToString(), (int)priority);
        }

        #endregion

        #region Configuration

        private static LoggingConfiguration LoggerConfig()
        {
            // Formatter 
            //TextFormatter formatter_long = new TextFormatter(@"Timestamp: {timestamp(local)}{newline}Message: {message}{newline}Category: {category}{newline}Priority: {priority}{newline}EventId: {eventid}{newline}ActivityId: {property(ActivityId)}{newline}Severity: {severity}{newline}Title: {title}{newline}");
            TextFormatter formatter_short = new TextFormatter(@"Timestamp: {timestamp(local)}{newline}Message: {message}{newline}");

            // TraceListener
            string logFileName = Path.Combine(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Log"), "all.log");
            if (File.Exists(logFileName)) { File.Delete(logFileName); }
            var flatFileTraceListener = new FlatFileTraceListener(logFileName, "", "", formatter_short);

            // Build Configuration 
            var config = new LoggingConfiguration();
            config.AddLogSource("Debug", SourceLevels.All, true).AddTraceListener(flatFileTraceListener);

            return config;
        }

        #endregion
    }
}