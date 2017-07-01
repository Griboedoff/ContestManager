using System;
using Core.DataBaseEntities;
using Core.Enums;
using Newtonsoft.Json;

namespace Core.Factories
{
    public interface IAuthenticationAccountFactory
    {
        AuthenticationAccount Create(User user, string userEmail, string userPassword);
    }

    public class AuthenticationAccountFactory : IAuthenticationAccountFactory
    {
        private readonly IServiceTokenFactory serviceTokenFactory;

        public AuthenticationAccountFactory(IServiceTokenFactory serviceTokenFactory)
        {
            this.serviceTokenFactory = serviceTokenFactory;
        }

        public AuthenticationAccount Create(User user, string userEmail, string userPassword)
        {
            var passwordToken = serviceTokenFactory.CreatePasswordToken(userPassword);

            return new AuthenticationAccount
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Type = AuthenticationType.Email,
                ServiceId = userEmail,
                ServiceToken = JsonConvert.SerializeObject(passwordToken)
            };
        }
    }
}