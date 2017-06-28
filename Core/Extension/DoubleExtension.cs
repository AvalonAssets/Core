using System;

namespace AvalonAssets.Core.Extension
{
    public static class DoubleExtension
    {
        /// <summary>
        ///     Returns true if two double are about equal.
        ///     Not using Double.Epsilon, read the reference for the reason.
        ///     Reference: http://stackoverflow.com/a/2411661/3673259
        /// </summary>
        /// <param name="left">Self.</param>
        /// <param name="right">Double to be compare.</param>
        /// <returns>Is two double about equal.</returns>
        public static bool AboutEqual(this double left, double right)
        {
            return Math.Abs(left - right) <= Math.Max(Math.Abs(left), Math.Abs(right)) * 1E-15;
        }

        /// <summary>
        ///     Returns true if two double are about equal or greater than.
        /// </summary>
        /// <param name="left">Self.</param>
        /// <param name="right">Double to be compare.</param>
        /// <returns>Is two double about equal or greater than.</returns>
        /// <seealso cref="AboutEqual" />
        public static bool AboutGreaterThanOrEqual(this double left, double right)
        {
            return left > right || AboutEqual(left, right);
        }

        /// <summary>
        ///     Returns true if two double are about equal or lesser than.
        /// </summary>
        /// <param name="left">Self.</param>
        /// <param name="right">Double to be compare.</param>
        /// <returns>Is two double about equal or lesser than.</returns>
        /// <seealso cref="AboutEqual" />
        public static bool AboutLesserThanOrEqual(this double left, double right)
        {
            return left < right || AboutEqual(left, right);
        }
    }
}