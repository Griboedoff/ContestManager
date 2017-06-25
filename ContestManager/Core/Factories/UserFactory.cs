using System;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Models;
using Newtonsoft.Json;

namespace Core.Factories
{
    public interface IUserFactory
    {
        (User, AuthenticationAccount) Create(EmailRegistrationRequest request);
    }

    public class UserFactory : IUserFactory
    {
        public (User, AuthenticationAccount) Create(EmailRegistrationRequest request)
        {
            var user = CreateUser(request);
            var account = CreateAccount(request, user);

            return (user, account);
        }

        private static User CreateUser(EmailRegistrationRequest request)
            => new User { Id = Guid.NewGuid(), Name = request.Name, Role = UserRole.User };

        private static string CreateEmailServiceToken(EmailRegistrationRequest request)
            => JsonConvert.SerializeObject(new EmailServiceToken(request));

        private static AuthenticationAccount CreateAccount(EmailRegistrationRequest request, User user)
            => new AuthenticationAccount
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Type = AuthenticationType.Email,
                ServiceId = request.EmailAddress,
                ServiceToken = CreateEmailServiceToken(request)
            };
    }
}