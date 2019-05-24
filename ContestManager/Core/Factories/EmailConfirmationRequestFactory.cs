using System;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Helpers;

namespace Core.Factories
{
    public interface IInviteEmailFactory
    {
        Invite CreateInvite(AuthenticationAccount account, ConfirmationType type);
        Invite CreateRestorePassword(AuthenticationAccount account, ConfirmationType type);
    }

    public class InviteEmailFactory : IInviteEmailFactory
    {
        private readonly IDataGenerator dataGenerator;

        public InviteEmailFactory(IDataGenerator dataGenerator)
        {
            this.dataGenerator = dataGenerator;
        }

        public Invite CreateInvite(AuthenticationAccount account, ConfirmationType type)
            => new Invite
            {
                Id = Guid.NewGuid(),
                Type = type,
                Email = account.ServiceId,
                ConfirmationCode = dataGenerator.GenerateSequence(FieldsLength.ConfirmationCode),
                IsUsed = false,
                AccountId = account.Id,
                PasswordRestore = false,
            };

        public Invite CreateRestorePassword(AuthenticationAccount account, ConfirmationType type)
            => new Invite
            {
                Id = Guid.NewGuid(),
                Type = type,
                Email = account.ServiceId,
                ConfirmationCode = dataGenerator.GenerateSequence(FieldsLength.ConfirmationCode),
                IsUsed = false,
                AccountId = account.Id,
                PasswordRestore = true,
            };
    }
}