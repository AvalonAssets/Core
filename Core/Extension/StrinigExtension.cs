using System;
using System.Text;

namespace AvalonAssets.Core.Extension
{
    public static class StrinigExtension
    {
        /// <summary>
        ///     Wrap the string to given length.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="wrapLength">Maximun length.</param>
        /// <param name="lineBreak">Line break character(s).</param>
        /// <param name="wrapLong">Wrap the word that is longer than <paramref name="wrapLength" /> even if there is no space.</param>
        /// <returns>Wrapped string.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="wrapLength" /> less than 1.</exception>
        public static string Wrap(this string source, int wrapLength, string lineBreak = null, bool wrapLong = true)
        {
            if (wrapLength < 1)
                throw new ArgumentOutOfRangeException($"{nameof(wrapLength)} less than 1.");
            if (lineBreak == null)
                lineBreak = Environment.NewLine;
            var offset = 0;
            var wrapped = new StringBuilder(source.Length);
            while (source.Length - offset > wrapLength)
            {
                if (source[offset] == ' ')
                {
                    offset++;
                    continue;
                }
                var spaceToWrapAt = source.LastIndexOf(' ', wrapLength + offset);
                if (spaceToWrapAt >= offset)
                {
                    wrapped.Append(source.Range(offset, spaceToWrapAt))
                        .Append(lineBreak);
                    offset = spaceToWrapAt + 1;
                    continue;
                }
                if (wrapLong)
                {
                    wrapped.Append(source.Range(offset, wrapLength + offset))
                        .Append(lineBreak);
                    offset += wrapLength;
                    continue;
                }
                spaceToWrapAt = source.IndexOf(' ', wrapLength + offset);
                if (spaceToWrapAt >= 0)
                {
                    wrapped.Append(source.Range(offset, spaceToWrapAt))
                        .Append(lineBreak);
                    offset = spaceToWrapAt + 1;
                }
                else
                {
                    wrapped.Append(source.Substring(offset));
                    offset = source.Length;
                }
            }
            wrapped.Append(source.Substring(offset));
            return wrapped.ToString();
        }

        /// <summary>
        ///     Retrieves a substring from this instance.
        ///     The substring starts at a specified character position and ends at a specified character position.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="startIndex">Start index.</param>
        /// <param name="endIndex">End index.</param>
        /// <returns>
        ///     A substring from this instance with given <paramref name="startIndex" /> and <paramref name="endIndex" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="startIndex" /> or <paramref name="endIndex" /> less than 0 or greater than the length of the source
        ///     string.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="endIndex" /> less than <paramref name="startIndex" />.</exception>
        public static string Range(this string source, int startIndex, int endIndex)
        {
            if (startIndex < 0 || startIndex >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (endIndex < 0 || endIndex >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(endIndex));
            if (endIndex < startIndex)
                throw new ArgumentException($"{nameof(endIndex)} less than {nameof(startIndex)}");
            var length = endIndex - startIndex;
            return source.Substring(startIndex, length);
        }
    }
}