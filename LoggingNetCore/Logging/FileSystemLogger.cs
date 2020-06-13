using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace LoggingNetCore.Logging
{
    public class FileSystemLogger : ILogger
    {
        private readonly FileSystemLoggerProvider provider;
        private readonly string category;

        private const string LOG_PATH = @"D:\Logs";

        public FileSystemLogger(FileSystemLoggerProvider loggerProvider, string categoryName)
        {
            provider = loggerProvider;
            category = categoryName;
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                if (!Directory.Exists(LOG_PATH))
                {
                    Directory.CreateDirectory(LOG_PATH);
                }

                var now = DateTime.UtcNow;
                var fileName = Path.Combine(LOG_PATH, $"{now:yyyyMMdd}.txt");

                var message = $"[{now:HH:mm:ss}] {logLevel} - {state}";
                if (exception != null)
                {
                    message += $"{Environment.NewLine}{exception}";
                }
                message += Environment.NewLine;

                File.AppendAllText(fileName, message);
            }
            catch
            {
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
