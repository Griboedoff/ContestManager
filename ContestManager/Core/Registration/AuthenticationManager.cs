using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Extensions;
using Core.Helpers;
using Core.Managers;
using Core.Models;
using Core.Models.Configs;
using Newtonsoft.Json;

namespace Core.Registration
{
    public interface IAuthenticationManager
    {
        Task<User> Authenticate(EmailLoginInfo loginInfo);
        Task<User> Authenticate(VkLoginInfo loginInfo);
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly VkAppConfig vkAppConfig;
        private readonly ICryptoHelper cryptoHelper;
        private readonly ISecurityManager securityManager;
        private readonly IAsyncRepository<AuthenticationAccount> accountsRepo;
        private readonly IAsyncRepository<User> usersRepo;

        public AuthenticationManager(
            VkAppConfig vkAppConfig,
            ICryptoHelper cryptoHelper,
            ISecurityManager securityManager,
            IAsyncRepository<AuthenticationAccount> accountsRepo,
            IAsyncRepository<User> usersRepo)
        {
            this.vkAppConfig = vkAppConfig;
            this.cryptoHelper = cryptoHelper;
            this.securityManager = securityManager;
            this.accountsRepo = accountsRepo;
            this.usersRepo = usersRepo;
        }

        public async Task<User> Authenticate(EmailLoginInfo loginInfo)
        {
            var account = await accountsRepo.FirstOrDefaultAsync(
                a => a.Type == AuthenticationType.Password && a.ServiceId == loginInfo.Email);

            if (account == default(AuthenticationAccount))
                throw new AuthenticationFailedException();

            var token = JsonConvert.DeserializeObject<PasswordToken>(account.ServiceToken);
            if (!securityManager.ValidatePassword(token, loginInfo.Password))
                throw new AuthenticationFailedException();

            return await usersRepo.GetByIdAsync(account.UserId);
        }

        public async Task<User> Authenticate(VkLoginInfo loginInfo)
        {
            var bytes =
                $"expire={loginInfo.Expire}mid={loginInfo.Mid}secret={loginInfo.Secret}sid={loginInfo.Sid}{vkAppConfig.SecretKey}"
                    .ToBytes();

            var md5Hash = cryptoHelper.ComputeMD5(bytes);
            if (md5Hash.ToHex() != loginInfo.Sig.ToUpper())
                throw new AuthenticationFailedException();

            var account = await accountsRepo.FirstOrDefaultAsync(
                a => a.Type == AuthenticationType.Vk && a.ServiceId == loginInfo.Mid);

            if (account == default(AuthenticationAccount))
                throw new AuthenticationFailedException();

            return await usersRepo.GetByIdAsync(account.UserId);
        }
    }
}