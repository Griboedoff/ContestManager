using System;
using System.Net;
using System.Net.Mail;
using Core.Models.Configs;
using Core.Models.Mails;

namespace Core.Managers
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

                Credentials = new NetworkCredential(config.SmtpUser, config.SmtpPwd),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
        }

        public void Send(EmailBase mail)
        {
            // todo xackill: дропнуть перед релизом
            if (mail.To != "teinlevi@gmail.com")
                throw new Exception("You can send only to teinlevi@gmail.com");

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