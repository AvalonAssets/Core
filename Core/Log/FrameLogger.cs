using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonAssets.Core.Extension;

namespace AvalonAssets.Core.Log
{
    /// <summary>
    ///     <see cref="FrameLogger" /> is <see cref="ILogger" /> decorator which format the output of log.
    ///     Also, if the logger implements <see cref="IPrefixLogger" /> prevent logging multiple message.
    ///     You can use different <see cref="Style" /> for different look of the frame.
    /// </summary>
    /// <example>
    ///     <see cref="ConsoleLogger" />:
    ///     <code>
    /// 2017-01-01T00:00:00:[Tag](Error): Message
    ///      </code>
    ///     <see cref="ConsoleLogger" /> with <see cref="FrameLogger" />:
    ///     <code>
    /// 2017-06-30T14:06:17:[Tag](Error): ╔════════════════════════════════════════════════════╗
    ///                                   ║ Tag: Tag                                           ║
    ///                                   ║ Level: Error                                       ║
    ///                                   ╟────────────────────────────────────────────────────╢
    ///                                   ║ Message                                            ║
    ///                                   ╚════════════════════════════════════════════════════╝
    ///      </code>
    /// </example>
    public class FrameLogger : ILogger
    {
        private readonly ILogger _logger;
        private readonly Style _style;

        /// <summary>
        ///     Create a new instance of <see cref="FrameLogger" /> with <paramref name="logger" />
        ///     and default <see cref="Style" />.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public FrameLogger(ILogger logger) : this(logger, new Style())
        {
        }

        /// <summary>
        ///     Create a new instance of <see cref="FrameLogger" /> with <paramref name="logger" />
        ///     and given <see cref="Style" />.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="style">Format style.</param>
        public FrameLogger(ILogger logger, Style style)
        {
            _logger = logger;
            _style = style;
        }

        /// <inheritdoc />
        public void Log(LogLevel logLevel, string tag, string message, Exception exception)
        {
            var entries = LogEntries(logLevel, tag, message, exception);
            var logger = _logger as IPrefixLogger;
            if (logger != null)
            {
                var prefix = logger.CalculatePrefixLenght(logLevel, tag);
                LogWithPrefix(prefix, logLevel, tag, entries);
            }
            else
            {
                LogWithoutPrefix(logLevel, tag, entries);
            }
        }

        private void LogWithPrefix(int prefix, LogLevel logLevel, string tag, IEnumerable<string> entries)
        {
            var entryList = entries as IList<string> ?? entries.ToList();
            var size = entryList.Count;
            var totalWidth = prefix + _style.TotalWidth;
            var messageBuilder = new StringBuilder();
            foreach (var entry in entryList.Select((value, index) => new {index, value}))
            {
                var value = entry.value;
                var index = entry.index;
                if (index != 0)
                    value = value.PadLeft(totalWidth);
                messageBuilder.Append(value);
                if (index != size - 1)
                    messageBuilder.Append(Environment.NewLine);
            }
            _logger.Log(logLevel, tag, messageBuilder.ToString());
        }

        private void LogWithoutPrefix(LogLevel logLevel, string tag, IEnumerable<string> entries)
        {
            foreach (var entry in entries)
                _logger.Log(logLevel, tag, entry);
        }

        private IEnumerable<string> LogEntries(LogLevel logLevel, string tag, string message, Exception exception)
        {
            yield return _style.CreateRoof();
            foreach (var entry in _style.CreateEntries($"Tag: {tag}"))
                yield return entry;
            foreach (var entry in _style.CreateEntries($"Level: {logLevel}"))
                yield return entry;
            yield return _style.CreateInnerWall();
            foreach (var entry in _style.CreateEntries(message))
                yield return entry;
            if (exception != null)
            {
                yield return _style.CreateInnerWall();
                foreach (var entry in _style.CreateEntries($"Exception: {exception.GetType()}"))
                    yield return entry;
                var exceptionMessage = exception.Message;
                if (!string.IsNullOrWhiteSpace(exceptionMessage))
                    foreach (var entry in _style.CreateEntries($"Message: {exceptionMessage}"))
                        yield return entry;
                var exceptionSource = exception.Source;
                if (!string.IsNullOrWhiteSpace(exceptionSource))
                    foreach (var entry in _style.CreateEntries($"Source: {exceptionSource}"))
                        yield return entry;
                yield return _style.CreateInnerWall();
                foreach (var entry in _style.CreateEntries($"StackTrace: {exception.StackTrace}"))
                    yield return entry;
            }
            yield return _style.CreateFloor();
        }

        public class Style
        {
            private const int MinWidth = 20;
            private const int MinPadding = 0;
            private int _padding = 1;
            private int _width = 50;

            /// <summary>
            ///     Default is ╚.
            /// </summary>
            public char BottomLeftCorner = '╚';

            /// <summary>
            ///     Default is ╝.
            /// </summary>
            public char BottomRightCorner = '╝';

            /// <summary>
            ///     Default is ─.
            /// </summary>
            public char HorizontalInnerlWall = '─';

            /// <summary>
            ///     Default is ═.
            /// </summary>
            public char HorizontalOutterWall = '═';

            /// <summary>
            ///     Default is ╟.
            /// </summary>
            public char LeftVerticalSplitter = '╟';

            /// <summary>
            ///     Default is ╢.
            /// </summary>
            public char RightVerticalSplitter = '╢';

            /// <summary>
            ///     Default is ╔.
            /// </summary>
            public char TopLeftCorner = '╔';

            /// <summary>
            ///     Default is ╗.
            /// </summary>
            public char TopRightCorner = '╗';

            /// <summary>
            ///     Default is ║.
            /// </summary>
            public char VerticalOutterWall = '║';

            internal int TotalWidth => TotalInnerWidth + 2;
            internal int TotalInnerWidth => Width + Padding * 2;

            /// <summary>
            ///     The maximum width of a message. It wraps long message.
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException">
            ///     Value less than 20.
            /// </exception>
            public int Width
            {
                get { return _width; }
                set
                {
                    if (value < MinWidth)
                        throw new ArgumentOutOfRangeException($"{nameof(Width)} cannot be smaller than {MinWidth}");
                    _width = value;
                }
            }

            /// <summary>
            ///     Padding between the message and the frame.
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException">
            ///     Value less than 0.
            /// </exception>
            public int Padding
            {
                get { return _padding; }
                set
                {
                    if (value < MinPadding)
                        throw new ArgumentOutOfRangeException($"{nameof(Padding)} cannot be smaller than {MinPadding}");
                    _padding = value;
                }
            }

            internal string CreateRoof()
            {
                return $"{TopLeftCorner}{new string(HorizontalOutterWall, TotalInnerWidth)}{TopRightCorner}";
            }

            internal string CreateInnerWall()
            {
                return
                    $"{LeftVerticalSplitter}{new string(HorizontalInnerlWall, TotalInnerWidth)}{RightVerticalSplitter}";
            }

            internal IEnumerable<string> CreateEntries(string content)
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    yield return $"{VerticalOutterWall}{new string(' ', TotalInnerWidth)}{VerticalOutterWall}";
                }
                else
                {
                    var wrapped = content.Wrap(Width);
                    foreach (var line in wrapped.Split(new[] {Environment.NewLine}, StringSplitOptions.None))
                        yield return $"{VerticalOutterWall} {line.PadRight(Width)} {VerticalOutterWall}";
                }
            }

            internal string CreateFloor()
            {
                return $"{BottomLeftCorner}{new string(HorizontalOutterWall, Width + Padding * 2)}{BottomRightCorner}";
            }
        }
    }
}