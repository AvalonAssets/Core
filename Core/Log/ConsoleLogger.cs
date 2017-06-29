using System;

namespace AvalonAssets.Core.Log
{
    public class ConsoleLogger : ILogger, IPrefixLogger
    {
        public void Log(LogLevel logLevel, string tag, string message, Exception exception)
        {
            var now = DateTime.Now;
            if (message != null)
                Console.WriteLine($"{now:s}:[{tag}]({logLevel}): {message}");
            if (exception != null)
                Console.WriteLine($"{now:s}:[{tag}]({logLevel}): {exception}");
        }

        public int CalculatePrefixLenght(LogLevel logLevel, string tag)
        {
            var now = DateTime.Now;
            return $"{now:s}:[{tag}]({logLevel}): ".Length;
        }
    }
}