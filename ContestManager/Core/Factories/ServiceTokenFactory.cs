using Core.Helpers;
using Core.Models;

namespace Core.Factories
{
    public interface IServiceTokenFactory
    {
        PasswordToken CreatePasswordToken(string userPassword);
    }

    public class ServiceTokenFactory : IServiceTokenFactory
    {
        private readonly ICryptoHelper cryptoHelper;
        private readonly IDataGenerator dataGenerator;

        public ServiceTokenFactory(ICryptoHelper cryptoHelper, IDataGenerator dataGenerator)
        {
            this.cryptoHelper = cryptoHelper;
            this.dataGenerator = dataGenerator;
        }

        public PasswordToken CreatePasswordToken(string userPassword)
        {
            var sult = dataGenerator.GenerateSequence(FieldsLength.Sult);
            var hash = cryptoHelper.ComputeSHA1(userPassword, sult);

            return new PasswordToken { Sult = sult, Base64Hash = hash };
        }
    }
}