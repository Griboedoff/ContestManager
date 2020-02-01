using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Extensions;
using Core.Helpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Users.Login
{
    public interface IAuthenticationManager
    {
        Task<User> Authenticate(EmailLoginInfo loginInfo);
        Task<User> Authenticate(VkLoginInfo loginInfo);
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly ILogger<AuthenticationManager> logger;
        private readonly VkAppConfig vkAppConfig;
        private readonly ICryptoHelper cryptoHelper;
        private readonly ISecurityManager securityManager;
        private readonly IAsyncRepository<AuthenticationAccount> accountsRepo;
        private readonly IAsyncRepository<User> usersRepo;

        public AuthenticationManager(
            ILogger<AuthenticationManager> logger,
            VkAppConfig vkAppConfig,
            ICryptoHelper cryptoHelper,
            ISecurityManager securityManager,
            IAsyncRepository<AuthenticationAccount> accountsRepo,
            IAsyncRepository<User> usersRepo)
        {
            this.logger = logger;
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
            {
                logger.LogWarning($"Не удалось войти по логин-паролю {loginInfo.Email}. Нет аккаунта.");
                throw new AuthenticationFailedException();
            }

            var token = JsonConvert.DeserializeObject<PasswordToken>(account.ServiceToken);
            if (!securityManager.ValidatePassword(token, loginInfo.Password))
            {
                logger.LogWarning($"Не удалось войти по логин-паролю {loginInfo.Email}. Неверный пароль");
                throw new AuthenticationFailedException();
            }

            logger.LogInformation($"Успешный вход по логин-паролю {loginInfo.Email}.");
            return await usersRepo.GetByIdAsync(account.UserId);
        }

        public async Task<User> Authenticate(VkLoginInfo loginInfo)
        {
            var bytes =
                $"expire={loginInfo.Expire}mid={loginInfo.Mid}secret={loginInfo.Secret}sid={loginInfo.Sid}{vkAppConfig.SecretKey}"
                    .ToBytes();

            var md5Hash = cryptoHelper.ComputeMD5(bytes);
            if (md5Hash.ToHex() != loginInfo.Sig.ToUpper())
            {
                logger.LogWarning($"Не удалось войти по VK {loginInfo.Mid}. Не совпала подпись");
                throw new AuthenticationFailedException();
            }

            var account = await accountsRepo.FirstOrDefaultAsync(
                a => a.Type == AuthenticationType.Vk && a.ServiceId == loginInfo.Mid);

            if (account == default(AuthenticationAccount))
            {
                logger.LogWarning($"Не удалось войти по VK {loginInfo.Mid}. Нет аккаунта.");
                throw new AuthenticationFailedException();
            }

            logger.LogInformation($"Успешный вход по VK {loginInfo.Mid}.");
            return await usersRepo.GetByIdAsync(account.UserId);
        }
    }
}
