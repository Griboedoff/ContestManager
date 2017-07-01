using System.Linq;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Enums.RequestStatuses;
using Core.Factories;
using Core.Models.Mails;

namespace Core.Managers
{
    public interface IRegistrationManager
    {
        RequestCreatingStatus CreateEmailRegistrationRequest(string userEmail);
        RequestConfirmingStatus ConfirmEmailRegistrationRequest(string userName, string userEmail, string userPassword, string confirmationCode);
    }

    public class RegistrationManager : IRegistrationManager
    {
        private readonly IUserFactory userFactory;
        private readonly IEmailManager emailManager;
        private readonly IContextAdapterFactory contextFactory;
        private readonly IEmailConfirmationRequestFactory emailConfirmationRequestFactory;
        private readonly IAuthenticationAccountFactory authenticationAccountFactory;

        public RegistrationManager(
            IUserFactory userFactory, 
            IEmailManager emailManager, 
            IContextAdapterFactory contextFactory, 
            IEmailConfirmationRequestFactory emailConfirmationRequestFactory, 
            IAuthenticationAccountFactory authenticationAccountFactory)
        {
            this.userFactory = userFactory;
            this.emailManager = emailManager;
            this.contextFactory = contextFactory;
            this.emailConfirmationRequestFactory = emailConfirmationRequestFactory;
            this.authenticationAccountFactory = authenticationAccountFactory;
        }

        public RequestCreatingStatus CreateEmailRegistrationRequest(string userEmail)
        {
            using (var db = contextFactory.Create())
            {
                if (IsEmailAddressAlreadyUsed(db, userEmail))
                    return RequestCreatingStatus.EmailAlreadyUsed;

                var request = emailConfirmationRequestFactory.Create(userEmail, ConfirmationType.Registration);
                SendEmail(request);

                db.AttachToInsert(request);
                db.SaveChanges();
            }

            return RequestCreatingStatus.Success;
        }

        public RequestConfirmingStatus ConfirmEmailRegistrationRequest(string userName, string userEmail, string userPassword, string confirmationCode)
        {
            using (var db = contextFactory.Create())
            {
                var request = db
                    .SetWithAttach<EmailConfirmationRequest>()
                    .FirstOrDefault(r => r.Type == ConfirmationType.Registration && r.EmailAddress == userEmail && r.ConfirmationCode == confirmationCode);

                if (request == null)
                    return RequestConfirmingStatus.WrongConfirmationCode;

                if (request.IsUsed)
                    return RequestConfirmingStatus.RequestAlreadyUsed;

                var user = userFactory.Create(userName, UserRole.User);
                db.AttachToInsert(user);

                var account = authenticationAccountFactory.Create(user, userEmail, userPassword);
                db.AttachToInsert(account);

                request.IsUsed = true;

                db.SaveChanges();
            }

            return RequestConfirmingStatus.Success;
        }

        private static bool IsEmailAddressAlreadyUsed(IContextAdapter db, string email)
            => db.Set<AuthenticationAccount>().Count(r => r.ServiceId == email) > 0;

        private void SendEmail(EmailConfirmationRequest request)
        {
            var mail = new RegistrationConfirmEmail(request.EmailAddress, request.ConfirmationCode);
            emailManager.Send(mail);
        }
    }
}