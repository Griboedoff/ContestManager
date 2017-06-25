using System;
using Core.DataBaseEntities;
using Core.Helpers;

namespace Core.Factories
{
    public interface IRegistrationRequestFactory
    {
        EmailRegistrationRequest CreateEmailRequest(string name, string email, string password);
    }

    public class RegistrationRequestFactory : IRegistrationRequestFactory
    {
        private readonly IDataGenerator dataGenerator;
        private readonly ICryptoHelper cryptoHelper;

        public RegistrationRequestFactory(IDataGenerator dataGenerator, ICryptoHelper cryptoHelper)
        {
            this.dataGenerator = dataGenerator;
            this.cryptoHelper = cryptoHelper;
        }

        public EmailRegistrationRequest CreateEmailRequest(string name, string email, string password)
        {
            var secret = dataGenerator.GenerateSequence(FieldsLength.Secret);
            var sult = dataGenerator.GenerateSequence(FieldsLength.Sult);

            return new EmailRegistrationRequest
            {
                Id = Guid.NewGuid(),
                Name = name,
                EmailAddress = email,
                Secret = secret,
                Sult = sult,
                PasswordHash = cryptoHelper.ComputeSHA1(password, sult),
                IsUsed = false
            };
        }
    }
}