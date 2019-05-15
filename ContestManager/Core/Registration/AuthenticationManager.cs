using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Extensions;
using Core.Helpers;
using Core.Managers;
using Core.Models;
using Core.Models.Configs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Core.Registration
{
    public interface IAuthenticationManager
    {
        Task<User> Authenticate(string email, string password);
        Task<User> Authenticate(VkLoginInfo loginInfo);
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly VkAppConfig vkAppConfig;
        private readonly ICryptoHelper cryptoHelper;
        private readonly ISecurityManager securityManager;

        public AuthenticationManager(
            VkAppConfig vkAppConfig,
            ICryptoHelper cryptoHelper,
            ISecurityManager securityManager)
        {
            this.vkAppConfig = vkAppConfig;
            this.cryptoHelper = cryptoHelper;
            this.securityManager = securityManager;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            using var db = new Context();

            var account = await db
                .AuthenticationAccounts
                .FirstOrDefaultAsync(a => a.Type == AuthenticationType.Password && a.ServiceId == email);

            if (account == default(AuthenticationAccount))
                throw new AuthenticationFailedException();

            var token = JsonConvert.DeserializeObject<PasswordToken>(account.ServiceToken);
            if (!securityManager.ValidatePassword(token, password))
                throw new AuthenticationFailedException();

            return await db.Users.Read(account.UserId);
        }

        public async Task<User> Authenticate(VkLoginInfo loginInfo)
        {
            var bytes =
                $"expire={loginInfo.Expire}mid={loginInfo.Mid}secret={loginInfo.Secret}sid={loginInfo.Sid}{vkAppConfig.SecretKey}"
                    .ToBytes();
            var md5Hash = cryptoHelper.ComputeMD5(bytes);

            if (md5Hash.ToHex() != loginInfo.Sig.ToUpper())
                throw new AuthenticationFailedException();

            using var db = new Context();

            var account = await db
                .AuthenticationAccounts
                .FirstOrDefaultAsync(a => a.Type == AuthenticationType.Vk && a.ServiceId == loginInfo.Mid);

            if (account == default(AuthenticationAccount))
                throw new AuthenticationFailedException();

            return await db.Users.Read(account.UserId);
        }
    }
}