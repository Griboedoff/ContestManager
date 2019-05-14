using System.Linq;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Extensions;
using Core.Factories;
using Core.Helpers;
using Core.Managers;
using Core.Models;
using Core.Models.Configs;
using Newtonsoft.Json;

namespace Core.Registration
{
    public interface IAuthenticationManager
    {
        User Authenticate(string email, string password);
        User Authenticate(VkLoginInfo loginInfo);
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly VkAppConfig vkAppConfig;
        private readonly ICryptoHelper cryptoHelper;
        private readonly ISecurityManager securityManager;
        private readonly IContextAdapterFactory contextFactory;

        public AuthenticationManager(
            VkAppConfig vkAppConfig,
            ICryptoHelper cryptoHelper,
            ISecurityManager securityManager,
            IContextAdapterFactory contextFactory)
        {
            this.vkAppConfig = vkAppConfig;
            this.cryptoHelper = cryptoHelper;
            this.contextFactory = contextFactory;
            this.securityManager = securityManager;
        }

        public User Authenticate(string email, string password)
        {
            using (var db = contextFactory.Create())
            {
                var account = db
                    .Set<AuthenticationAccount>()
                    .FirstOrDefault(a => a.Type == AuthenticationType.Password && a.ServiceId == email);

                if (account == default(AuthenticationAccount))
                    throw new AuthenticationFailedException();

                var token = JsonConvert.DeserializeObject<PasswordToken>(account.ServiceToken);
                if (!securityManager.ValidatePassword(token, password))
                    throw new AuthenticationFailedException();

                return db.Read<User>(account.UserId);
            }
        }

        public User Authenticate(VkLoginInfo loginInfo)
        {
            var bytes = $"expire={loginInfo.Expire}mid={loginInfo.Mid}secret={loginInfo.Secret}sid={loginInfo.Sid}{vkAppConfig.SecretKey}".ToBytes();
            var md5Hash = cryptoHelper.ComputeMD5(bytes);

            if (md5Hash.ToHex() != loginInfo.Sig.ToUpper())
                throw new AuthenticationFailedException();

            using (var db = contextFactory.Create())
            {
                var account = db
                    .Set<AuthenticationAccount>()
                    .FirstOrDefault(a => a.Type == AuthenticationType.Vk && a.ServiceId == loginInfo.Mid);

                if (account == default(AuthenticationAccount))
                    throw new AuthenticationFailedException();

                return db.Read<User>(account.UserId);
            }
        }
    }
}