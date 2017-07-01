using System;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Helpers;

namespace Core.Factories
{
    public interface IEmailConfirmationRequestFactory
    {
        EmailConfirmationRequest Create(string email, ConfirmationType type);
    }

    public class EmailConfirmationRequestFactory : IEmailConfirmationRequestFactory
    {
        private readonly IDataGenerator dataGenerator;

        public EmailConfirmationRequestFactory(IDataGenerator dataGenerator)
        {
            this.dataGenerator = dataGenerator;
        }

        public EmailConfirmationRequest Create(string email, ConfirmationType type)
            => new EmailConfirmationRequest
            {
                Id = Guid.NewGuid(),
                Type = type,
                Email = email,
                ConfirmationCode = dataGenerator.GenerateSequence(FieldsLength.ConfirmationCode),
                IsUsed = false
            };
    }
}