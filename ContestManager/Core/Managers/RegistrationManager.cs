using System.Linq;
using Core.DataBaseEntities;
using Core.Exceptions;
using Core.Factories;

namespace Core.Managers
{
    public interface IRegistrationManager
    {
        void AddEmailRegistrationRequest(EmailRegistrationRequest request);
    }

    public class RegistrationManager : IRegistrationManager
    {
        private readonly IContextAdapterFactory contextFactory;

        public RegistrationManager(IContextAdapterFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void AddEmailRegistrationRequest(EmailRegistrationRequest request)
        {
            using (var db = contextFactory.Create())
            {
                var existing = db
                    .Set<EmailRegistrationRequest>()
                    .FirstOrDefault(r => r.Email == request.Email);

                if (existing != default(EmailRegistrationRequest))
                    throw new EmailAlreadyRegisteredException(request.Email);
                
                db.AttachToInsert(request);
                db.SaveChanges();
            }
        }
    }
}