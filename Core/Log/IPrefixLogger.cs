namespace AvalonAssets.Core.Log
{
    /// <summary>
    ///     <see cref="IPrefixLogger" /> mean the logger has a fix length prefix.
    /// </summary>
    /// <seealso cref="FrameLogger" />
    public interface IPrefixLogger
    {
        /// <summary>
        ///     Returns the length of a prefix of a log message.
        /// </summary>
        /// <param name="logLevel">Log level.</param>
        /// <param name="tag">Tag of the log.</param>
        /// <returns>Length of the prefix.</returns>
        int CalculatePrefixLenght(LogLevel logLevel, string tag);
    }
}