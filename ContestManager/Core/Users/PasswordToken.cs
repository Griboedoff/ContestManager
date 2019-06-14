namespace Core.Users
{
    public class PasswordToken
    {
        public string Salt { get; set; }
        public string Base64Hash { get; set; }
    }
}