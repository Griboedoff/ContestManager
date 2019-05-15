namespace Core.Models.Mails
{
    public abstract class EmailBase
    {
        public abstract string To { get; }

        public abstract string Subject { get; }
        public abstract string Message { get; }
    }
}