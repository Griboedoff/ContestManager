using System;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Managers;
using Newtonsoft.Json;

namespace Core.Factories
{
    public interface IAuthenticationAccountFactory
    {
        AuthenticationAccount CreatePasswordAuthenticationAccount(User user, string email, string password);
        AuthenticationAccount ChangePassword(AuthenticationAccount account, string newPassword);
        AuthenticationAccount CreateVkAuthenticationAccount(User user, string vkId);
    }

    public class AuthenticationAccountFactory : IAuthenticationAccountFactory
    {
        private readonly ISecurityManager securityManager;

        public AuthenticationAccountFactory(ISecurityManager securityManager)
        {
            this.securityManager = securityManager;
        }

        public AuthenticationAccount CreatePasswordAuthenticationAccount(User user, string email, string password)
        {
            var passwordToken = securityManager.CreatePasswordToken(password);

            return new AuthenticationAccount
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Type = AuthenticationType.Password,
                ServiceId = email,
                ServiceToken = JsonConvert.SerializeObject(passwordToken),
                IsActive = false,
            };
        }

        public AuthenticationAccount ChangePassword(AuthenticationAccount account, string newPassword)
        {
            var passwordToken = securityManager.CreatePasswordToken(newPassword);
            account.ServiceToken = JsonConvert.SerializeObject(passwordToken);

            return account;
        }

        public AuthenticationAccount CreateVkAuthenticationAccount(User user, string vkId)
        {
            return new AuthenticationAccount
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Type = AuthenticationType.Vk,
                ServiceId = vkId,
                IsActive = true,
            };
        }
    }
}