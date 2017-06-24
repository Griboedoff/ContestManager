using System.Text;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string str)
            => Encoding.UTF8.GetBytes(str);
    }
}