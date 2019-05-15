using System;
using System.Linq;
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
        Task<RegistrationStatus> CreateEmailRegistrationRequest(string email);
        Task<RegistrationStatus> RegisterByVk(string name, string vkId);
        Task FillFields(Guid userId, FieldWithValue[] fields);
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

        public async Task<RegistrationStatus> CreateEmailRegistrationRequest(string email)
        {
            if (await IsServiceIdAlreadyUsed(email))
                return RegistrationStatus.EmailAlreadyUsed;

            var request = emailConfirmationRequestFactory.Create(email, ConfirmationType.Registration);
            SendEmail(request);

            await confirmationRequestsRepo.AddAsync(request);

            return RegistrationStatus.RequestCreated;
        }

        public async Task<RegistrationStatus> RegisterByVk(string name, string vkId)
        {
            if (await IsServiceIdAlreadyUsed(vkId))
                return RegistrationStatus.VkIdAlreadyUsed;

            var user = Create(name, UserRole.User);
            await usersRepo.AddAsync(user);

            var account = authenticationAccountFactory.CreateVkAuthenticationAccount(user, vkId);
            await authenticationAccountRepo.AddAsync(account);

            return RegistrationStatus.Success;
        }


        public async Task FillFields(Guid userId, FieldWithValue[] fields)
        {
            var user = await usersRepo.GetByIdAsync(userId);
            var userFields = user.Fields.ToList();
            foreach (var userField in userFields)
                foreach (var field in fields)
                    if (userField.Title == field.Title)
                        userField.Value = field.Value;
            userFields.AddRange(fields.Where(f => !user.Fields.Contains(f)));

            user.Fields = userFields.ToArray();
            await usersRepo.UpdateAsync(user);
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
                Fields = new FieldWithValue[0],
            };
        }
    }
}