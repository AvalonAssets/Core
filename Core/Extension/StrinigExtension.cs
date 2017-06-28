using System;
using System.Text;

namespace AvalonAssets.Core.Extension
{
    public static class StrinigExtension
    {
        public static string Wrap(this string source, int wrapLength, string lineBreak, bool wrapLong = true)
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

        public static string Wrap(this string source, int wrapLength, bool wrapLong = true)
        {
            return source.Wrap(wrapLength, null, wrapLong);
        }

        public static string Range(this string source, int startIndex, int endIndex)
        {
            if (startIndex < 0 || startIndex >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (endIndex < 0 || endIndex >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(endIndex));
            if (endIndex < startIndex)
                throw new ArgumentOutOfRangeException($"{nameof(endIndex)} less than {nameof(startIndex)}");
            var length = endIndex - startIndex;
            return source.Substring(startIndex, length);
        }
    }
}