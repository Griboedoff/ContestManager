using System.Net;
using System.Net.Mail;
using Core.Mail.Models;

namespace Core.Mail
{
    public interface IEmailManager
    {
        void Send(EmailBase mail);
    }

    public class EmailManager : IEmailManager
    {
        private readonly SmtpClient client;
        private readonly string mailFrom;

        public EmailManager(EmailConfig config)
        {
            mailFrom = config.EmailboxAddr;

            client = new SmtpClient
            {
                Host = config.SmtpHost,
                Port = config.SmtpPort,

                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(config.SmtpUser, config.SmtpPwd),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
            };
        }

        public void Send(EmailBase mail)
        {
            using (var message = new MailMessage())
            {
                message.IsBodyHtml = true;
                message.From = new MailAddress(mailFrom, mail.Subject);

                message.To.Add(new MailAddress(mail.To));

                message.Subject = mail.Subject;
                message.Body = mail.Message;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(message);
            }
        }
    }
}
