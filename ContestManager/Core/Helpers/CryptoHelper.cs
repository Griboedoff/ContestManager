using System.Security.Cryptography;
using Core.Extensions;
using Core.Models.Configs;

namespace Core.Helpers
{
    public interface ICryptoHelper
    {
        byte[] ComputeMD5(byte[] data);
        byte[] ComputeSHA1(byte[] data);

        byte[] ComputeDetachedSign(byte[] data);
        bool VerifyDetachedSign(byte[] data, byte[] sign);
    }

    public class CryptoHelper : ICryptoHelper
    {
        private readonly byte[] key;

        public CryptoHelper(RSACryptoConfig cryptoConfig)
        {
            key = cryptoConfig.Base64Key.FromBase64();
        }

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

        public byte[] ComputeDetachedSign(byte[] data)
        {
            using (var csp = GetCryptoProvider())
                return csp.SignData(data, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }

        public bool VerifyDetachedSign(byte[] data, byte[] sign)
        {
            using (var csp = GetCryptoProvider())
                return csp.VerifyData(data, sign, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }

        private RSACryptoServiceProvider GetCryptoProvider()
        {
            var provider = new RSACryptoServiceProvider();
            provider.ImportCspBlob(key);
            return provider;
        }
    }
}