using System;

namespace Core.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBase64(this byte[] bytes)
            => Convert.ToBase64String(bytes);

        public static string ToHex(this byte[] bytes)
            => BitConverter.ToString(bytes).Replace("-", "");
    }
}