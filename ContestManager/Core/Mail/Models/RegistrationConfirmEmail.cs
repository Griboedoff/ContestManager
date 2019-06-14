using Core.Configs;
using Microsoft.Extensions.Options;

namespace Core.Mail.Models
{
    public class RegistrationConfirmEmail : EmailBase
    {
        public override string To { get; }
        private readonly string secretCode;
        private readonly IOptions<ConfigOptions> options;

        public RegistrationConfirmEmail(string destination, string secretCode, IOptions<ConfigOptions> options)
        {
            To = destination;
            this.secretCode = secretCode;
            this.options = options;
        }

        public override string Subject
            => "Вузак. Подтверждение регистрации";

        public override string Message
        {
            get
            {
                var inviteLink = $"{options.Value.SiteAddress}/invite/{secretCode}";
                return "Здравствуйте!<br/>" +
                       "<br/>" +
                       $"Этот адрес электронной почты был указан при регистрации на сайте <a href=\"{options.Value.SiteAddress}\">Вузовской-академической олимпиады</a><br/>" +
                       $"Для активации аккаунта пройдите по ссылке: <a href=\"{inviteLink}\">{inviteLink}</a><br/>" +
                       "Если это письмо пришло Вам по ошибке &mdash; просто проигнорируйте его.<br/>" +
                       "<br/>" +
                       "<hr/>" +
                       $"Администрация сайта <a href=\"{options.Value.SiteAddress}\">Вузовской-академической олимпиады</a>";
            }
        }
    }
}