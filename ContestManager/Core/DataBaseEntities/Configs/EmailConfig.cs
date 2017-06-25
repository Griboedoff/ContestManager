using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataBaseEntities.Configs
{
    public class EmailConfig : DataBaseEntity
    {
        [Column]
        public string MailboxAddress { get; set; }

        [Column]
        public string SmtpHost { get; set; }

        [Column]
        public int SmtpPort { get; set; }

        [Column]
        public string SmtpUser { get; set; }

        [Column]
        public string SmtpPwd { get; set; }
    }
}