using System.Security.Cryptography;

namespace Core.Helpers
{
    public interface ICryptoHelper
    {
        byte[] ComputeMD5(byte[] data);
        byte[] ComputeSHA1(byte[] data);
    }

    public class CryptoHelper : ICryptoHelper
    {
        public byte[] ComputeMD5(byte[] data)
        {
            using (var md5 = MD5.Create())
                return md5.ComputeHash(data);
        }

        public byte[] ComputeSHA1(byte[] data)
        {
            using (var sha = new SHA1Managed())
                return sha.ComputeHash(data);
        }
    }
}