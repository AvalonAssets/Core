using System;
using System.Collections.Generic;

namespace AvalonAssets.Core.Extension
{
    public static class ListExtension
    {
        public static IList<T> GetRange<T>(this IList<T> list, int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException($"{nameof(index)} is less than 0.");
            if (count < 0)
                throw new ArgumentOutOfRangeException($"{nameof(count)} is less than 0.");
            var result = list as List<T>;
            if (result != null)
                return result.GetRange(index, count);
            result = new List<T>();
            for (var i = 0; i < count; i++)
                result.Add(list[index + i]);
            return result;
        }

        public static void RemoveRange<T>(this IList<T> list, int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException($"{nameof(index)} is less than 0.");
            if (count < 0)
                throw new ArgumentOutOfRangeException($"{nameof(count)} is less than 0.");
            var result = list as List<T>;
            if (result != null)
                result.RemoveRange(index, count);
            else
            {
                for (var i = 0; i < count; i++)
                    list.RemoveAt(index);
            }
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> enumerable)
        {
            var result = list as List<T>;
            if (result != null)
                result.AddRange(enumerable);
            else
            {
                foreach (var value in enumerable)
                    list.Add(value);
            }
        }
    }
}