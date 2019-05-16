using System;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Helpers;

namespace Core.Factories
{
    public interface IEmailConfirmationRequestFactory
    {
        EmailConfirmationRequest Create(AuthenticationAccount email, ConfirmationType type);
    }

    public class EmailConfirmationRequestFactory : IEmailConfirmationRequestFactory
    {
        private readonly IDataGenerator dataGenerator;

        public EmailConfirmationRequestFactory(IDataGenerator dataGenerator)
        {
            this.dataGenerator = dataGenerator;
        }

        public EmailConfirmationRequest Create(AuthenticationAccount account, ConfirmationType type)
            => new EmailConfirmationRequest
            {
                Id = Guid.NewGuid(),
                Type = type,
                Email = account.ServiceId,
                ConfirmationCode = dataGenerator.GenerateSequence(FieldsLength.ConfirmationCode),
                IsUsed = false,
                AccountId = account.Id,
            };
    }
}