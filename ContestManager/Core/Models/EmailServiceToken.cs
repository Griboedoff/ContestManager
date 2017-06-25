using Core.DataBaseEntities;

namespace Core.Models
{
    public class EmailServiceToken
    {
        public readonly string Sult;
        public readonly byte[] PasswordHash;

        public EmailServiceToken(EmailRegistrationRequest request)
        {
            Sult = request.Sult;
            PasswordHash = request.PasswordHash;
        }
    }
}