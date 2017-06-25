using System.Linq;
using Core.DataBase;
using Core.DataBaseEntities;
using Core.Enums.RequestStatuses;
using Core.Factories;
using Core.Models.Mails;

namespace Core.Managers
{
    public interface IRegistrationManager
    {
        RegistrationRequestResult AddEmailRegistrationRequest(string userName, string userEmail, string userPassword);
        ConfirmRequestResult ConfirmEmailRegistration(string userEmail, string secret);
    }

    public class RegistrationManager : IRegistrationManager
    {
        private readonly IUserFactory userFactory;
        private readonly IEmailManager emailManager;
        private readonly IContextAdapterFactory contextFactory;
        private readonly IRegistrationRequestFactory registrationRequestFactory;

        public RegistrationManager(
            IUserFactory userFactory, 
            IEmailManager emailManager, 
            IContextAdapterFactory contextFactory, 
            IRegistrationRequestFactory registrationRequestFactory)
        {
            this.userFactory = userFactory;
            this.emailManager = emailManager;
            this.contextFactory = contextFactory;
            this.registrationRequestFactory = registrationRequestFactory;
        }

        public RegistrationRequestResult AddEmailRegistrationRequest(string userName, string userEmail, string userPassword)
        {
            using (var db = contextFactory.Create())
            {
                if (IsEmailAddressAlreadyUsed(db, userEmail))
                    return RegistrationRequestResult.EmailAddressAlreadyUsed;

                var request = registrationRequestFactory.CreateEmailRequest(userName, userEmail, userPassword);
                var mail = new RegistrationConfirmEmail(userEmail, request.Secret);
                emailManager.Send(mail);

                db.AttachToInsert(request);
                db.SaveChanges();
            }

            return RegistrationRequestResult.Success;
        }

        public ConfirmRequestResult ConfirmEmailRegistration(string userEmail, string secret)
        {
            using (var db = contextFactory.Create())
            {
                var request = db
                    .SetWithAttach<EmailRegistrationRequest>()
                    .FirstOrDefault(r => r.EmailAddress == userEmail && r.Secret == secret);

                if (request == null)
                    return ConfirmRequestResult.WrongConfirmCode;

                //todo xackill: поиспользовать логику с IsUsed
                //if (request.IsUsed)
                //    return ConfirmRequestResult.WrongConfirmCode;

                var (user, account) = userFactory.Create(request);

                request.IsUsed = true;
                db.AttachToInsert(user);
                db.AttachToInsert(account);
                db.SaveChanges();
            }

            return ConfirmRequestResult.Success;
        }

        private static bool IsEmailAddressAlreadyUsed(IContextAdapter db, string email)
            => db.Set<AuthenticationAccount>().Count(r => r.ServiceId == email) > 0;
    }
}