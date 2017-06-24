using System.Security.Cryptography;
using Core.Extensions;

namespace Core.Helpers
{
    public interface ICryptoHelper
    {
        byte[] ComputeSHA1(string password, string sult);
    }

    public class CryptoHelper : ICryptoHelper
    {
        public byte[] ComputeSHA1(string password, string sult)
        {
            var bytes = (password + sult).ToBytes();

            using (var sha = new SHA1Managed())
                return sha.ComputeHash(bytes);
        }
    }
}