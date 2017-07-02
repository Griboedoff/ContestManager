using System.Linq;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.DataBaseEnums;
using Core.Enums.RequestStatuses;
using Core.Factories;
using Core.Models.Mails;

namespace Core.Managers
{
    public interface IRegistrationManager
    {
        RegistrationStatus CreateEmailRegistrationRequest(string email);
        RegistrationStatus ConfirmEmailRegistrationRequest(string name, string email, string password, string confirmationCode);

        RegistrationStatus RegisterByVk(string name, string vkId);
    }

    public class RegistrationManager : IRegistrationManager
    {
        private readonly IUserFactory userFactory;
        private readonly IEmailManager emailManager;
        private readonly IContextAdapterFactory contextFactory;
        private readonly IAuthenticationAccountFactory authenticationAccountFactory;
        private readonly IEmailConfirmationRequestFactory emailConfirmationRequestFactory;

        public RegistrationManager(
            IUserFactory userFactory, 
            IEmailManager emailManager, 
            IContextAdapterFactory contextFactory, 
            IAuthenticationAccountFactory authenticationAccountFactory, 
            IEmailConfirmationRequestFactory emailConfirmationRequestFactory)
        {
            this.userFactory = userFactory;
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

        public RegistrationStatus ConfirmEmailRegistrationRequest(string name, string email, string password, string confirmationCode)
        {
            using (var db = contextFactory.Create())
            {
                var request = db
                    .SetWithAttach<EmailConfirmationRequest>()
                    .FirstOrDefault(r => r.Type == ConfirmationType.Registration && r.Email == email && r.ConfirmationCode == confirmationCode);

                if (request == null)
                    return RegistrationStatus.WrongConfirmationCode;

                if (request.IsUsed)
                    return RegistrationStatus.RequestAlreadyUsed;

                var user = userFactory.Create(name, UserRole.User);
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

                var user = userFactory.Create(name, UserRole.User);
                db.AttachToInsert(user);

                var account = authenticationAccountFactory.CreateVkAuthenticationAccount(user, vkId);
                db.AttachToInsert(account);

                db.SaveChanges();
            }

            return RegistrationStatus.Success;
        }

        private static bool IsServiceIdAlreadyUsed(IContextAdapter db, string serviceId)
            => db.Set<AuthenticationAccount>().Count(r => r.ServiceId == serviceId) > 0;

        private void SendEmail(EmailConfirmationRequest request)
        {
            var mail = new RegistrationConfirmEmail(request.Email, request.ConfirmationCode);
            emailManager.Send(mail);
        }
    }
}