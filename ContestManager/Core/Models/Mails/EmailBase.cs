using Core.Helpers;

namespace Core.Models.Mails
{
    public abstract class EmailBase
    {
        public abstract string To { get; }

        public abstract string Subject { get; }
        public abstract string Message { get; }

        protected string Footer
            => "<br/>" +
               "<hr/>" +
               "С уважением,<br/>" +
               $"Администрация сайта <a href=\"{Secret.SiteAddress}\">ContestManager</a>";
    }
}