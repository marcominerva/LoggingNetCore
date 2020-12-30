using Microsoft.Extensions.Logging;
using System;

namespace LoggingNetCore.Logging
{
    [ProviderAlias("FileSystem")]
    public class FileSystemLoggerProvider : ILoggerProvider
    {
        // Create an instance of an ILogger, which is used to actually write the logs
        public ILogger CreateLogger(string categoryName) => new FileSystemLogger(this, categoryName);

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
