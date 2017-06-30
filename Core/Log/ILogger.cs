using System;

namespace AvalonAssets.Core.Log
{
    /// <summary>
    ///     <see cref="ILogger" /> is a logger interface for logging message.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///     Log the <paramref name="message" /> and <see cref="exception" />
        /// </summary>
        /// <param name="logLevel">Log level.</param>
        /// <param name="tag">Tag of the log message.</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Throw exception. Can be null.</param>
        void Log(LogLevel logLevel, string tag, string message, Exception exception);
    }
}