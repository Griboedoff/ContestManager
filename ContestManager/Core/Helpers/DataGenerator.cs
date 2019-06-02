using System;
using System.Linq;
using System.Security.Cryptography;
using Core.Extensions;

namespace Core.Helpers
{
    public interface IDataGenerator
    {
        string GenerateSequence(int len);
        string GenerateSequence(int len, string source);
    }

    public class DataGenerator : IDataGenerator
    {
        public const string EnglishUppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Numbers = "0123456789";

        private static readonly string Symbols = $"{EnglishUppercase}{EnglishUppercase.ToLower()}{Numbers}";

        private readonly RNGCryptoServiceProvider random;

        public DataGenerator() => random = new RNGCryptoServiceProvider();

        public string GenerateSequence(int len) => GenerateSequence(len, Symbols);

        public string GenerateSequence(int len, string source)
        {
            if (len < 1)
                throw new ArgumentException($"{nameof(len)} must be 1 or grater");

            var rndBytes = new byte[len];
            random.GetNonZeroBytes(rndBytes);

            return rndBytes
                .Select(b => source[b % source.Length])
                .JoinToString();
        }
    }
}