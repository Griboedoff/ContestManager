using Core.Configs;
using Microsoft.Extensions.Options;

namespace Core.Mail.Models
{
    public class PasswordRestoreEmail : EmailBase
    {
        public override string To { get; }
        private readonly string secretCode;
        private readonly IOptions<ConfigOptions> options;

        public PasswordRestoreEmail(string destination, string secretCode, IOptions<ConfigOptions> options)
        {
            To = destination;
            this.secretCode = secretCode;
            this.options = options;
        }

        public override string Subject => "Вузак. Восстановление пароля";

        public override string Message
        {
            get
            {
                var inviteLink = $"{options.Value.SiteAddress}/invite/{secretCode}";
                return "Здравствуйте!<br/>" +
                       "<br/>" +
                       $"Для восстановления пароля пройдите по ссылке: <a href=\"{inviteLink}\">{inviteLink}</a><br/>" +
                       "Если это письмо пришло Вам по ошибке &mdash; просто проигнорируйте его.<br/>" +
                       "<br/>" +
                       "<hr/>" +
                       $"Администрация сайта <a href=\"{options.Value.SiteAddress}\">Вузовской-академической олимпиады</a>";
            }
        }
    }
}