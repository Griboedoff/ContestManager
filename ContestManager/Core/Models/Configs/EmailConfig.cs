namespace Core.Models.Configs
{
    public class EmailConfig : IConfig
    {
        public string EmailboxAddr { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUser { get; set; }

        public string SmtpPwd { get; set; }
    }
}