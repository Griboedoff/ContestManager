using System;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Enums.RequestStatuses;
using Core.Factories;
using Core.Managers;
using Core.Models;
using Core.Models.Mails;
using Microsoft.Extensions.Options;

namespace Core.Registration
{
    public interface IUserManager
    {
        Task<RegistrationStatus> CreateEmailRegistrationRequest(EmailRegisterInfo email);
        Task<RegistrationStatus> RegisterByVk(string name, string vkId);
    }

    public class UserManager : IUserManager
    {
        private readonly IEmailManager emailManager;
        private readonly IAuthenticationAccountFactory authenticationAccountFactory;
        private readonly IEmailConfirmationRequestFactory emailConfirmationRequestFactory;
        private readonly IAsyncRepository<EmailConfirmationRequest> confirmationRequestsRepo;
        private readonly IAsyncRepository<User> usersRepo;
        private readonly IAsyncRepository<AuthenticationAccount> authenticationAccountRepo;
        private readonly IOptions<ConfigOptions> options;

        public UserManager(
            IEmailManager emailManager,
            IAuthenticationAccountFactory authenticationAccountFactory,
            IEmailConfirmationRequestFactory emailConfirmationRequestFactory,
            IAsyncRepository<EmailConfirmationRequest> confirmationRequestsRepo,
            IAsyncRepository<User> usersRepo,
            IAsyncRepository<AuthenticationAccount> authenticationAccountRepo,
            IOptions<ConfigOptions> options)
        {
            this.emailManager = emailManager;
            this.authenticationAccountFactory = authenticationAccountFactory;
            this.emailConfirmationRequestFactory = emailConfirmationRequestFactory;
            this.confirmationRequestsRepo = confirmationRequestsRepo;
            this.usersRepo = usersRepo;
            this.authenticationAccountRepo = authenticationAccountRepo;
            this.options = options;
        }

        public async Task<RegistrationStatus> CreateEmailRegistrationRequest(EmailRegisterInfo emailInfo)
        {
            if (await IsServiceIdAlreadyUsed(emailInfo.Email))
                return RegistrationStatus.EmailAlreadyUsed;

            var user = Create(GetName(emailInfo), UserRole.User);
            await usersRepo.AddAsync(user);

            var account = authenticationAccountFactory.CreatePasswordAuthenticationAccount(
                user,
                emailInfo.Email,
                emailInfo.Password);
            await authenticationAccountRepo.AddAsync(account);

            var request = emailConfirmationRequestFactory.Create(account, ConfirmationType.Registration);
            SendEmail(request);

            await confirmationRequestsRepo.AddAsync(request);

            return RegistrationStatus.RequestCreated;
        }

        public async Task<RegistrationStatus> RegisterByVk(string name, string vkId)
        {
            if (await IsServiceIdAlreadyUsed(vkId))
                return RegistrationStatus.VkIdAlreadyUsed;

            var user = await usersRepo.AddAsync(Create(name, UserRole.User));

            var account = authenticationAccountFactory.CreateVkAuthenticationAccount(user, vkId);
            await authenticationAccountRepo.AddAsync(account);

            return RegistrationStatus.Success;
        }

        private async Task<bool> IsServiceIdAlreadyUsed(string serviceId)
            => await authenticationAccountRepo.AnyAsync(r => r.ServiceId == serviceId);

        private void SendEmail(EmailConfirmationRequest request)
        {
            var mail = new RegistrationConfirmEmail(request.Email, request.ConfirmationCode, options);
            emailManager.Send(mail);
        }

        private static User Create(string userName, UserRole role)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Name = userName,
                Role = role,
            };
        }

        private static string GetName(EmailRegisterInfo emailInfo)
        {
            var name = $"{emailInfo.Surname} {emailInfo.Name}";
            if (!string.IsNullOrEmpty(emailInfo.Patronymic))
                name += $" {emailInfo.Patronymic}";
            return name;
        }
    }
}