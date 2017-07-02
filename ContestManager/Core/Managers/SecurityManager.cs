using Core.Extensions;
using Core.Helpers;
using Core.Models;

namespace Core.Managers
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
            var sult = dataGenerator.GenerateSequence(FieldsLength.Sult);
            var hash = GetHash(userPassword, sult);

            return new PasswordToken { Sult = sult, Base64Hash = hash.ToBase64() };
        }

        public bool ValidatePassword(PasswordToken token, string password)
        {
            var hash = GetHash(password, token.Sult);
            return hash.ToBase64() == token.Base64Hash;
        }

        private byte[] GetHash(string password, string sult)
            => cryptoHelper.ComputeSHA1((password + sult).ToBytes());
    }
}