using System.Collections.Generic;

namespace Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static string JoinToString<T>(this IEnumerable<T> arr, string separator = "")
            => string.Join(separator, arr);
    }
}