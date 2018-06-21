using System;
using System.Linq;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Enums.RequestStatuses;
using Core.Factories;
using Core.Models;
using Core.Models.Mails;

namespace Core.Managers
{
    public interface IUserManager
    {
        RegistrationStatus CreateEmailRegistrationRequest(string email);

        RegistrationStatus ConfirmEmailRegistrationRequest(string name, string email, string password,
            string confirmationCode);

        RegistrationStatus RegisterByVk(string name, string vkId);
        void FillFields(Guid userId, FieldWithValue[] fields);
    }

    public class UserManager : IUserManager
    {
        private readonly IEmailManager emailManager;
        private readonly IContextAdapterFactory contextFactory;
        private readonly IAuthenticationAccountFactory authenticationAccountFactory;
        private readonly IEmailConfirmationRequestFactory emailConfirmationRequestFactory;

        public UserManager(
            IEmailManager emailManager,
            IContextAdapterFactory contextFactory,
            IAuthenticationAccountFactory authenticationAccountFactory,
            IEmailConfirmationRequestFactory emailConfirmationRequestFactory)
        {
            this.emailManager = emailManager;
            this.contextFactory = contextFactory;
            this.authenticationAccountFactory = authenticationAccountFactory;
            this.emailConfirmationRequestFactory = emailConfirmationRequestFactory;
        }

        public RegistrationStatus CreateEmailRegistrationRequest(string email)
        {
            using (var db = contextFactory.Create())
            {
                if (IsServiceIdAlreadyUsed(db, email))
                    return RegistrationStatus.EmailAlreadyUsed;

                var request = emailConfirmationRequestFactory.Create(email, ConfirmationType.Registration);
                SendEmail(request);

                db.AttachToInsert(request);
                db.SaveChanges();
            }

            return RegistrationStatus.RequestCreated;
        }

        public RegistrationStatus ConfirmEmailRegistrationRequest(string name, string email, string password,
            string confirmationCode)
        {
            using (var db = contextFactory.Create())
            {
                var request = db
                              .SetWithAttach<EmailConfirmationRequest>()
                              .FirstOrDefault(r =>
                                  r.Type == ConfirmationType.Registration && r.Email == email &&
                                  r.ConfirmationCode == confirmationCode);

                if (request == null)
                    return RegistrationStatus.WrongConfirmationCode;

                if (request.IsUsed)
                    return RegistrationStatus.RequestAlreadyUsed;

                var user = Create(name, UserRole.User);
                db.AttachToInsert(user);

                var account = authenticationAccountFactory.CreatePasswordAuthenticationAccount(user, email, password);
                db.AttachToInsert(account);

                request.IsUsed = true;

                db.SaveChanges();
            }

            return RegistrationStatus.Success;
        }

        public RegistrationStatus RegisterByVk(string name, string vkId)
        {
            using (var db = contextFactory.Create())
            {
                if (IsServiceIdAlreadyUsed(db, vkId))
                    return RegistrationStatus.VkIdAlreadyUsed;

                var user = Create(name, UserRole.User);
                db.AttachToInsert(user);

                var account = authenticationAccountFactory.CreateVkAuthenticationAccount(user, vkId);
                db.AttachToInsert(account);

                db.SaveChanges();
            }

            return RegistrationStatus.Success;
        }

        public void FillFields(Guid userId, FieldWithValue[] fields)
        {
            using (var db = contextFactory.Create())
            {
                var user = db.FindAndAttach<User>(userId);
                var userFields = user.Fields.ToList();
                foreach (var userField in userFields)
                    foreach (var field in fields)
                        if (userField.Title == field.Title)
                            userField.Value = field.Value;
                userFields.AddRange(fields.Where(f => !user.Fields.Contains(f)));

                user.Fields = userFields.ToArray();
                db.SaveChanges();
            }
        }

        private static bool IsServiceIdAlreadyUsed(IContextAdapter db, string serviceId)
            => db.Set<AuthenticationAccount>().Any(r => r.ServiceId == serviceId);

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