using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Core.Extension
{
    public static class EnumExtension
    {
        /// <summary>
        ///     Gets all the values of enum <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>All enum value of <typeparamref name="T" /></returns>
        public static IEnumerable<T> Values<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        ///     Shifts the <paramref name="enumerable" /> by <paramref name="offset" />.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="enumerable">Enumerable to be shifted.</param>
        /// <param name="offset">Amount of shift.</param>
        /// <returns>Shifted enumerable.</returns>
        public static IEnumerable<T> Shift<T>(this IEnumerable<T> enumerable, int offset)
        {
            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (offset < 0)
            {
                var quotient = Math.Abs(offset) / list.Count + 1;
                offset += quotient * list.Count;
            }
            offset %= list.Count;
            for (var i = offset; i < list.Count; i++)
                yield return list[i];
            for (var i = 0; i < offset; i++)
                yield return list[i];
        }
    }
}