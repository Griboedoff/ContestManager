using System;
using System.Linq;
using System.Security.Cryptography;
using Core.Extensions;

namespace Core.Helpers
{
    public interface IDataGenerator
    {
        string GenerateSequence(int len);
    }

    public class DataGenerator : IDataGenerator
    {
        private const string Symbols = "AaBbCcDdEeFfGgHhiJjKkLMmNnPpQqRrSsTtUuVvWwXxYyZz";
        private readonly RNGCryptoServiceProvider random;

        public DataGenerator()
        {
            random = new RNGCryptoServiceProvider();
        }

        public string GenerateSequence(int len)
        {
            if (len < 1)
                throw new ArgumentException($"{nameof(len)} must be 1 or grater");

            var rndBytes = new byte[len];
            random.GetNonZeroBytes(rndBytes);

            return rndBytes
                .Select(b => Symbols[b % Symbols.Length])
                .JoinToString();
        }
    }
}