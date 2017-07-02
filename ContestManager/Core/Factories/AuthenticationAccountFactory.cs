using System;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Managers;
using Newtonsoft.Json;

namespace Core.Factories
{
    public interface IAuthenticationAccountFactory
    {
        AuthenticationAccount CreatePasswordAuthenticationAccount(User user, string userEmail, string userPassword);
        AuthenticationAccount CreateVkAuthenticationAccount(User user, string vkId);
    }

    public class AuthenticationAccountFactory : IAuthenticationAccountFactory
    {
        private readonly ISecurityManager securityManager;

        public AuthenticationAccountFactory(ISecurityManager securityManager)
        {
            this.securityManager = securityManager;
        }

        public AuthenticationAccount CreatePasswordAuthenticationAccount(User user, string userEmail, string userPassword)
        {
            var passwordToken = securityManager.CreatePasswordToken(userPassword);

            return new AuthenticationAccount
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Type = AuthenticationType.Password,
                ServiceId = userEmail,
                ServiceToken = JsonConvert.SerializeObject(passwordToken)
            };
        }

        public AuthenticationAccount CreateVkAuthenticationAccount(User user, string vkId)
        {
            return new AuthenticationAccount
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Type = AuthenticationType.Vk,
                ServiceId = vkId,
            };
        }
    }
}