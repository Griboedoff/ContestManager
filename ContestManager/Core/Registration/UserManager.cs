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
using Microsoft.EntityFrameworkCore;

namespace Core.Registration
{
    public interface IUserManager
    {
        Task<RegistrationStatus> CreateEmailRegistrationRequest(string email);

        Task<RegistrationStatus> ConfirmEmailRegistrationRequest(
            string name,
            string email,
            string password,
            string confirmationCode);

        Task<RegistrationStatus> RegisterByVk(string name, string vkId);
        Task FillFields(Guid userId, FieldWithValue[] fields);
    }

    public class UserManager : IUserManager
    {
        private readonly IEmailManager emailManager;
        private readonly IAuthenticationAccountFactory authenticationAccountFactory;
        private readonly IEmailConfirmationRequestFactory emailConfirmationRequestFactory;

        public UserManager(
            IEmailManager emailManager,
            IAuthenticationAccountFactory authenticationAccountFactory,
            IEmailConfirmationRequestFactory emailConfirmationRequestFactory)
        {
            this.emailManager = emailManager;
            this.authenticationAccountFactory = authenticationAccountFactory;
            this.emailConfirmationRequestFactory = emailConfirmationRequestFactory;
        }

        public async Task<RegistrationStatus> CreateEmailRegistrationRequest(string email)
        {
            using var db = new Context();

            if (await IsServiceIdAlreadyUsed(email))
                return RegistrationStatus.EmailAlreadyUsed;

            var request = emailConfirmationRequestFactory.Create(email, ConfirmationType.Registration);
            SendEmail(request);

            db.EmailConfirmationRequests.Add(request);
            await db.SaveChangesAsync();

            return RegistrationStatus.RequestCreated;
        }

        public async Task<RegistrationStatus> ConfirmEmailRegistrationRequest(
            string name,
            string email,
            string password,
            string confirmationCode)
        {
            using var db = new Context();

            var request = await db
                .EmailConfirmationRequests
                .FirstOrDefaultAsync(
                    r =>
                        r.Type == ConfirmationType.Registration && r.Email == email &&
                        r.ConfirmationCode == confirmationCode);

            if (request == null)
                return RegistrationStatus.WrongConfirmationCode;

            if (request.IsUsed)
                return RegistrationStatus.RequestAlreadyUsed;

            var user = Create(name, UserRole.User);
            db.Users.Add(user);

            var account = authenticationAccountFactory.CreatePasswordAuthenticationAccount(user, email, password);
            db.AuthenticationAccounts.Add(account);

            request.IsUsed = true;
            await db.SaveChangesAsync();

            return RegistrationStatus.Success;
        }

        public async Task<RegistrationStatus> RegisterByVk(string name, string vkId)
        {
            using var db = new Context();

            if (await IsServiceIdAlreadyUsed(vkId))
                return RegistrationStatus.VkIdAlreadyUsed;

            var user = Create(name, UserRole.User);
            db.Users.Add(user);

            var account = authenticationAccountFactory.CreateVkAuthenticationAccount(user, vkId);
            db.AuthenticationAccounts.Add(account);

            await db.SaveChangesAsync();

            return RegistrationStatus.Success;
        }

        public async Task FillFields(Guid userId, FieldWithValue[] fields)
        {
            using var db = new Context();

            var user = db.Users.Find(userId);
            var userFields = user.Fields.ToList();
            foreach (var userField in userFields)
                foreach (var field in fields)
                    if (userField.Title == field.Title)
                        userField.Value = field.Value;
            userFields.AddRange(fields.Where(f => !user.Fields.Contains(f)));

            user.Fields = userFields.ToArray();
            await db.SaveChangesAsync();
        }

        private static async Task<bool> IsServiceIdAlreadyUsed(string serviceId)
        {
            using var db = new Context();

            return await db.Set<AuthenticationAccount>().AnyAsync(r => r.ServiceId == serviceId);
        }

        private void SendEmail(EmailConfirmationRequest request)
        {
            var mail = new RegistrationConfirmEmail(request.Email, request.ConfirmationCode);
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