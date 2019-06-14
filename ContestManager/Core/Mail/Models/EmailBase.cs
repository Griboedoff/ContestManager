namespace Core.Mail.Models
{
    public abstract class EmailBase
    {
        public abstract string To { get; }

        public abstract string Subject { get; }
        public abstract string Message { get; }
    }
}