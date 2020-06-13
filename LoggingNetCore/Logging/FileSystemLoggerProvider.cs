using Microsoft.Extensions.Logging;

namespace LoggingNetCore.Logging
{
    [ProviderAlias("FileSystem")]
    public class FileSystemLoggerProvider : ILoggerProvider
    {
        // Create an instance of an ILogger, which is used to actually write the logs
        public ILogger CreateLogger(string categoryName)
        {
            return new FileSystemLogger(this, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
