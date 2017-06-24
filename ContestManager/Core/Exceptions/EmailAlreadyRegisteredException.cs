using System;

namespace Core.Exceptions
{
    public class EmailAlreadyRegisteredException : Exception
    {
        public EmailAlreadyRegisteredException(string email)
            : base($"Email {email} already registered")
        {
        }
    }
}