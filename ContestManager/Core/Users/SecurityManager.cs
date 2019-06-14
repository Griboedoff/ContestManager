using Core.Extensions;
using Core.Helpers;

namespace Core.Users
{
    public interface ISecurityManager
    {
        PasswordToken CreatePasswordToken(string userPassword);
        bool ValidatePassword(PasswordToken token, string password);
    }

    public class SecurityManager : ISecurityManager
    {
        private readonly ICryptoHelper cryptoHelper;
        private readonly IDataGenerator dataGenerator;

        public SecurityManager(ICryptoHelper cryptoHelper, IDataGenerator dataGenerator)
        {
            this.cryptoHelper = cryptoHelper;
            this.dataGenerator = dataGenerator;
        }

        public PasswordToken CreatePasswordToken(string userPassword)
        {
            var salt = dataGenerator.GenerateSequence(FieldsLength.Salt);
            var hash = GetHash(userPassword, salt);

            return new PasswordToken { Salt = salt, Base64Hash = hash.ToBase64() };
        }

        public bool ValidatePassword(PasswordToken token, string password)
        {
            var hash = GetHash(password, token.Salt);
            return hash.ToBase64() == token.Base64Hash;
        }

        private byte[] GetHash(string password, string salt)
            => cryptoHelper.ComputeSHA1($"{password}{salt}".ToBytes());
    }
}