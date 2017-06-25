using Core.Helpers;

namespace Core.Models.Mails
{
    public class RegistrationConfirmEmail : EmailBase
    {
        public override string To { get; }
        private readonly string secretCode;

        public RegistrationConfirmEmail(string destination, string secretCode)
        {
            To = destination;
            this.secretCode = secretCode;
        }

        public override string Subject
            => "ContestManager. Подтверждение регистрации";

        public override string Message
            =>      "Здравствуйте!<br/>" +
                    "<br/>" +
                   $"Этот адрес электронной почты был указан при регистрации на сайте <a href=\"{Secret.SiteAddress}\">ContestManager</a><br/>" +
                   $"Для активации аккаунта, используйте секретный код: <strong>{secretCode}</strong><br/>" +
                    "Если это письмо пришло Вам по ошибке &mdash; просто проигнорируйте его.<br/>" +
                    Footer;
    }
}