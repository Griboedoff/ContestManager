using System;
using System.Linq;
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

        private readonly Random random;

        public DataGenerator()
        {
            random = new Random((int)DateTime.Now.Ticks);
        }

        public string GenerateSequence(int len)
        {
            if (len < 1)
                throw new ArgumentException($"{nameof(len)} must be 1 or grater");

            return Enumerable
                .Range(1, len)
                .Select(num => Symbols[random.Next(1, 9999) % Symbols.Length])
                .JoinToString();
        }
    }
}