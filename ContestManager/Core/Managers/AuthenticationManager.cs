using System.Linq;
using System.Security.Authentication;
using System.Text;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Exceptions;
using Core.Extensions;
using Core.Factories;
using Core.Helpers;
using Core.Models;
using Core.Models.Configs;
using Newtonsoft.Json;

namespace Core.Managers
{
    public interface IAuthenticationManager
    {
        User Authenticate(string email, string password);
        User Authenticate(long expire, string mid, string secret, string sid, string sig);
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly VkAppConfig vkAppConfig;
        private readonly ICryptoHelper cryptoHelper;
        private readonly ISecurityManager securityManager;
        private readonly IContextAdapterFactory contextFactory;

        public AuthenticationManager(VkAppConfig vkAppConfig, ICryptoHelper cryptoHelper, ISecurityManager securityManager, IContextAdapterFactory contextFactory)
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

        public User Authenticate(long expire, string mid, string secret, string sid, string sig)
        {
            var bytes = $"expire={expire}mid={mid}secret={secret}sid={sid}{vkAppConfig.SecretKey}".ToBytes();
            var md5Hash = cryptoHelper.ComputeMD5(bytes);

            if (md5Hash.ToHex() != sig.ToUpper())
                throw new AuthenticationFailedException();

            using (var db = contextFactory.Create())
            {
                var account = db
                    .Set<AuthenticationAccount>()
                    .FirstOrDefault(a => a.Type == AuthenticationType.Vk && a.ServiceId == mid);

                if (account == default(AuthenticationAccount))
                    throw new AuthenticationFailedException();
                
                return db.Read<User>(account.UserId);
            }
        }
    }
}